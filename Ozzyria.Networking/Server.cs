using Ozzyria.Game;
using Ozzyria.Game.Components;
using Grecs;
using Ozzyria.Networking.Model;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Ozzyria.Networking
{
    public class Server
    {
        const int SERVER_PORT = 13000;

        const float SECONDS_PER_TICK = 0.016f;
        const int TIMEOUT_MINUTES = 2;
        const int MAX_CLIENTS = 8;

        private readonly IPEndPoint[] clients;
        private readonly DateTime[] clientLastHeardFrom;

        private readonly UdpClient server;
        private readonly World world;

        public Server()
        {
            clients = new IPEndPoint[MAX_CLIENTS];
            clientLastHeardFrom = new DateTime[MAX_CLIENTS];

            // TODO OZ-28 add abstract so World is configure in Ozzyria.Server and leave Networking package just for networking
            world = new World();
            server = new UdpClient(SERVER_PORT);
        }

        public void Start(object obj = null)
        {
            CancellationToken ct;
            if (obj != null)
                ct = (CancellationToken)obj;
            else
                ct = new CancellationToken();

            try
            {
                Console.WriteLine($"Server Started - Listening on port {SERVER_PORT}");
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Restart();
                var isRunning = true;
                while (isRunning && !ct.IsCancellationRequested)
                {
                    // TODO OZ-28 chunk entity updates sent back
                    // TODO OZ-28 move sending/reading entity updates into separate tasks on the client/server
                    HandleMessages();

                    if (stopWatch.ElapsedMilliseconds >= SECONDS_PER_TICK * 1000)
                    {
                        // update and re-send state every SECONDS_PER_TICK
                        world.Update(SECONDS_PER_TICK);
                        SendLocalState();
                        SendGlobalState();

                        stopWatch.Restart();
                    }
                }
            }
            finally {
                server.Close();
                Console.WriteLine("Server Stopped");
            }
        }

        private void HandleMessages()
        {
            while (server.Available > 0)
            {
                try
                {
                    var clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
                    var packet = ClientPacketFactory.Parse(server.Receive(ref clientEndPoint));
                    var messageType = packet.Type;
                    var messageClient = packet.ClientId;
                    var messageData = packet.Data;
                

                    switch (messageType)
                    {
                        case ClientMessage.Join:
                            var clientId = PrepareClientSlot(clientEndPoint);
                            var joinPacket = ServerPacketFactory.Join(clientId);
                            server.Send(joinPacket, joinPacket.Length, clientEndPoint);
                            break;
                        case ClientMessage.Leave:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                clients[messageClient] = null;
                                world.PlayerLeave(messageClient);
                                Console.WriteLine($"Client #{messageClient} Left");
                            }
                            break;
                        case ClientMessage.InputUpdate:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                var input = ClientPacketFactory.ParseInputData(messageData);
                                world.WorldState.PlayerInputBuffer[messageClient].MoveUp = input.MoveUp;
                                world.WorldState.PlayerInputBuffer[messageClient].MoveDown = input.MoveDown;
                                world.WorldState.PlayerInputBuffer[messageClient].MoveLeft = input.MoveLeft;
                                world.WorldState.PlayerInputBuffer[messageClient].MoveRight = input.MoveRight;
                                world.WorldState.PlayerInputBuffer[messageClient].Attack = input.Attack;

                                clientLastHeardFrom[messageClient] = DateTime.Now;
                            }
                            break;
                        case ClientMessage.OpenBag:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                var bagEntityId = ClientPacketFactory.ParseOpenBagData(messageData);
                                var areaContext = world.GetLocalContext(messageClient);
                                var bagEntity = areaContext.GetEntity(bagEntityId);
                                if(bagEntity == null || !bagEntity.HasComponent(typeof(Game.Components.Bag)))
                                {
                                    // cannot open bag
                                    var bagContentsPacket = ServerPacketFactory.CannotOpenBagContents(bagEntityId);
                                    SendToClient(messageClient, bagContentsPacket);
                                }
                                else
                                {
                                    // send back bag contents
                                    var bag = (Game.Components.Bag)bagEntity.GetComponent(typeof(Game.Components.Bag));
                                    var bagContentsPacket = ServerPacketFactory.BagContents(bagEntityId, bag.Contents.ToArray());
                                    SendToClient(messageClient, bagContentsPacket);
                                }

                                clientLastHeardFrom[messageClient] = DateTime.Now;
                            }
                            break;
                        case ClientMessage.EquipItem:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                var bagItemRequest = ClientPacketFactory.ParseEquipItemData(messageData);
                                var areaContext = world.GetLocalContext(messageClient);

                                // TODO OZ-28 add check to make sure that Player has access to the bag!!
                                var bagEntity = areaContext.GetEntity(bagItemRequest.BagEntityId);
                                if (bagEntity == null || !bagEntity.HasComponent(typeof(Game.Components.Bag)))
                                {
                                    // cannot open bag
                                    var bagContentsPacket = ServerPacketFactory.CannotOpenBagContents(bagItemRequest.BagEntityId);
                                    SendToClient(messageClient, bagContentsPacket);
                                }
                                else
                                {
                                    var sourceBag = (Bag)bagEntity.GetComponent(typeof(Bag));
                                    if (bagItemRequest.ItemSlot >= 0 && bagItemRequest.ItemSlot < sourceBag.Contents.Count)
                                    {
                                        Bag bag = sourceBag;
                                        var bagId = bagItemRequest.BagEntityId;
                                        var itemSlot = bagItemRequest.ItemSlot;
                                        var playerEntity = world.WorldState.Areas[world.WorldState.PlayerAreaTracker[messageClient]]._context.GetEntities(new EntityQuery().And(typeof(Game.Components.Player))).FirstOrDefault(e => ((Game.Components.Player)e.GetComponent(typeof(Game.Components.Player))).PlayerId == messageClient);
                                        if (playerEntity.id != bagId)
                                        {
                                            var playerBag = (Bag)playerEntity.GetComponent(typeof(Bag));
                                            var tranferredEntity = sourceBag.RemoveItem(itemSlot);

                                            if (tranferredEntity != null)
                                            {
                                                playerBag.AddItem(tranferredEntity);

                                                bagId = playerEntity.id;
                                                itemSlot = playerBag.Contents.Count - 1;

                                                // equip from players bag now that item is transferred
                                                bag = playerBag;
                                            }
                                        }

                                        var itemEntity = bag.Contents[itemSlot];
                                        var item = (Item)itemEntity.GetComponent(typeof(Item));

                                        // unequip gear currently in equipment slot
                                        var equippedItems = bag.Contents.Where(i =>
                                        {
                                            var ii = i.GetComponent(typeof(Item)) as Item;
                                            return ii != null && ii.IsEquipped && ii.EquipmentSlot == item.EquipmentSlot;
                                        });
                                        foreach(var equippedItem in equippedItems)
                                        {
                                            ((Item)equippedItem.GetComponent(typeof(Item))).IsEquipped = false;
                                        }

                                        // equip gear into appropriate slot
                                        var weapon = (Weapon)playerEntity.GetComponent(typeof(Weapon));
                                        var hat = (Hat)playerEntity.GetComponent(typeof(Hat));
                                        var armor = (Armor)playerEntity.GetComponent(typeof(Armor));
                                        var mask = (Mask)playerEntity.GetComponent(typeof(Mask));
                                        switch (item.EquipmentSlot)
                                        {
                                            case "hat":
                                                hat.HatId = item.ItemId;
                                                break;
                                            case "armor":
                                                armor.ArmorId = item.ItemId;
                                                break;
                                            case "mask":
                                                mask.MaskId = item.ItemId;
                                                break;
                                            case "weapon":
                                                // TODO OZ-55 support more than swords? (load weapon based on itemId!)
                                                weapon.WeaponType = WeaponType.Sword;
                                                weapon.WeaponId = item.ItemId;
                                                break;
                                        }
                                        item.IsEquipped = true;

                                        // send source bag contents back
                                        var bagContentsPacket = ServerPacketFactory.BagContents(bagEntity.id, sourceBag.Contents.ToArray());
                                        SendToClient(messageClient, bagContentsPacket);
                                        if(bagEntity.id != bagId)
                                        {
                                            // also send player inventory back to client
                                            bagContentsPacket = ServerPacketFactory.BagContents(bagId, bag.Contents.ToArray());
                                            SendToClient(messageClient, bagContentsPacket);
                                        }
                                    }
                                    else
                                    {
                                        // cannot open bag
                                        var bagContentsPacket = ServerPacketFactory.CannotOpenBagContents(bagItemRequest.BagEntityId);
                                        SendToClient(messageClient, bagContentsPacket);
                                    }
                                }

                                clientLastHeardFrom[messageClient] = DateTime.Now;
                            }
                            break;
                        case ClientMessage.UnequipItem:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                var bagItemRequest = ClientPacketFactory.ParseUnequipItemData(messageData);
                                var areaContext = world.GetLocalContext(messageClient);

                                var bagEntity = areaContext.GetEntity(bagItemRequest.BagEntityId);
                                var playerComponent = (Player)bagEntity?.GetComponent(typeof(Player));
                                if (bagEntity == null || playerComponent == null || !bagEntity.HasComponent(typeof(Game.Components.Bag)))
                                {
                                    // cannot open bag
                                    var bagContentsPacket = ServerPacketFactory.CannotOpenBagContents(bagItemRequest.BagEntityId);
                                    SendToClient(messageClient, bagContentsPacket);
                                }
                                else
                                {
                                    var bag = (Bag)bagEntity.GetComponent(typeof(Bag));

                                    if (bagItemRequest.ItemSlot >= 0 && bagItemRequest.ItemSlot < bag.Contents.Count)
                                    {
                                        var itemEntity = bag.Contents[bagItemRequest.ItemSlot];
                                        var item = (Item)itemEntity.GetComponent(typeof(Item));

                                        // unequip gear from the appropriate slot
                                        var weapon = (Weapon)bagEntity.GetComponent(typeof(Weapon));
                                        var hat = (Hat)bagEntity.GetComponent(typeof(Hat));
                                        var armor = (Armor)bagEntity.GetComponent(typeof(Armor));
                                        var mask = (Mask)bagEntity.GetComponent(typeof(Mask));
                                        switch (item.EquipmentSlot)
                                        {
                                            case "hat":
                                                hat.HatId = "";
                                                break;
                                            case "armor":
                                                armor.ArmorId = "";
                                                break;
                                            case "mask":
                                                mask.MaskId = "";
                                                break;
                                            case "weapon":
                                                weapon.WeaponType = WeaponType.Empty;
                                                weapon.WeaponId = "";
                                                break;
                                        }
                                        item.IsEquipped = false;

                                        // send source bag contents back
                                        var bagContentsPacket = ServerPacketFactory.BagContents(bagEntity.id, bag.Contents.ToArray());
                                        SendToClient(messageClient, bagContentsPacket);
                                    }
                                    else
                                    {
                                        // cannot open bag
                                        var bagContentsPacket = ServerPacketFactory.CannotOpenBagContents(bagItemRequest.BagEntityId);
                                        SendToClient(messageClient, bagContentsPacket);
                                    }
                                }

                                clientLastHeardFrom[messageClient] = DateTime.Now;
                            }
                            break;
                        case ClientMessage.DropItem:
                            if (IsValidEndPoint(messageClient, clientEndPoint))
                            {
                                var bagItemRequest = ClientPacketFactory.ParseDropItemData(messageData);
                                var areaContext = world.GetLocalContext(messageClient);

                                var bagEntity = areaContext.GetEntity(bagItemRequest.BagEntityId);
                                var playerComponent = (Player)bagEntity?.GetComponent(typeof(Player));
                                if (bagEntity == null || playerComponent == null || !bagEntity.HasComponent(typeof(Game.Components.Bag)))
                                {
                                    // cannot open bag
                                    var bagContentsPacket = ServerPacketFactory.CannotOpenBagContents(bagItemRequest.BagEntityId);
                                    SendToClient(messageClient, bagContentsPacket);
                                }
                                else
                                {
                                    var bag = (Bag)bagEntity.GetComponent(typeof(Bag));

                                    if (bagItemRequest.ItemSlot >= 0 && bagItemRequest.ItemSlot < bag.Contents.Count)
                                    {
                                        var itemEntity = bag.Contents[bagItemRequest.ItemSlot];
                                        var item = (Item)itemEntity.GetComponent(typeof(Item));

                                        // unequip gear from the appropriate slot
                                        if (item.IsEquipped)
                                        {
                                            var weapon = (Weapon)bagEntity.GetComponent(typeof(Weapon));
                                            var hat = (Hat)bagEntity.GetComponent(typeof(Hat));
                                            var armor = (Armor)bagEntity.GetComponent(typeof(Armor));
                                            var mask = (Mask)bagEntity.GetComponent(typeof(Mask));
                                            switch (item.EquipmentSlot)
                                            {
                                                case "hat":
                                                    hat.HatId = "";
                                                    break;
                                                case "armor":
                                                    armor.ArmorId = "";
                                                    break;
                                                case "mask":
                                                    mask.MaskId = "";
                                                    break;
                                                case "weapon":
                                                    weapon.WeaponType = WeaponType.Empty;
                                                    weapon.WeaponId = "";
                                                    break;
                                            }
                                            item.IsEquipped = false;
                                        }

                                        var droppedEntity = bag.RemoveItem(bagItemRequest.ItemSlot);
                                        if (droppedEntity != null)
                                        {
                                            var bagMovement = (Movement)bagEntity.GetComponent(typeof(Movement));

                                            // Create Bag
                                            var newBagEntity = areaContext.CreateEntity();

                                            var newBag = (Bag)newBagEntity.CreateComponent(typeof(Bag));
                                            newBag.Name = "Bag";
                                            newBag.Capacity = 4;
                                            newBag.AddItem(droppedEntity);
                                            newBagEntity.AddComponent(newBag);

                                            var newBagMovement = (Movement)newBagEntity.CreateComponent(typeof(Movement));
                                            newBagMovement.X = bagMovement.X;
                                            newBagMovement.Y = bagMovement.Y;
                                            newBagMovement.PreviousX = bagMovement.X;
                                            newBagMovement.PreviousY = bagMovement.Y;
                                            newBagEntity.AddComponent(newBagMovement);

                                            newBagEntity.AddComponent(new Skeleton { Type = SkeletonType.Static });
                                            newBagEntity.AddComponent(new Animator { Type = ClipType.Stall });

                                            areaContext.AttachEntity(newBagEntity);
                                        }

                                        // send source bag contents back
                                        var bagContentsPacket = ServerPacketFactory.BagContents(bagEntity.id, bag.Contents.ToArray());
                                        SendToClient(messageClient, bagContentsPacket);
                                    }
                                    else
                                    {
                                        // cannot open bag
                                        var bagContentsPacket = ServerPacketFactory.CannotOpenBagContents(bagItemRequest.BagEntityId);
                                        SendToClient(messageClient, bagContentsPacket);
                                    }
                                }

                                clientLastHeardFrom[messageClient] = DateTime.Now;
                            }
                            break;
                    }
                }
                catch (SocketException)
                {
                    // Likely a client connection was reset (will get cleaned up by clientLastHeardFrom)
                }
            }
        }

        private int PrepareClientSlot(IPEndPoint clientEndPoint)
        {
            int clientId = 0;
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i))
                {
                    clientId = i;
                    clients[i] = clientEndPoint;
                    clientLastHeardFrom[i] = DateTime.Now;
                    world.PlayerJoin(i);
                    Console.WriteLine($"Client #{i} Joined");
                    break;
                }
            }

            return clientId;
        }


        private void SendLocalState()
        {
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (!IsConnected(i))
                {
                    continue;
                }

                var localContext = world.GetLocalContext(i);
                if(localContext == null)
                {
                    continue;
                }

                // TODO OZ-28 add better mechanism for broadcasting packates in local areas to players in those areas
                // TODO OZ-28 get rid of this event system of make it less coupled to the "World" and/or les boxing/unboxing of objects
                foreach(var areaEvent in world.WorldState.AreaEvents)
                {
                    if(areaEvent is EntityLeaveAreaEvent)
                    {
                        EntityLeaveAreaEvent alae = (EntityLeaveAreaEvent)areaEvent;
                        if (alae.PlayerId == i) {
                            // tell client the local player left the area they were in
                            var areaChangePacket = ServerPacketFactory.AreaChanged(i, alae.SourceArea, alae.NewArea);
                            SendToClient(i, areaChangePacket);
                        }
                    }
                }

                var entityPacket = ServerPacketFactory.EntityUpdates(localContext.GetEntities());
                SendToClient(i, entityPacket);

                var destroyPacket = ServerPacketFactory.EntityRemovals(localContext);
                SendToClient(i, destroyPacket);
            }

            world.WorldState.AreaEvents.Clear();
        }

        private void SendGlobalState()
        {
            // TODO Send Globally Broadcasted Packets
        }

        private void SendToAll(byte[] packet, int exclude = -1)
        {
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (exclude == i)
                {
                    continue;
                }

                SendToClient(i, packet);
            }
        }

        private void SendToClient(int clientId, byte[] packet)
        {
            if (!IsConnected(clientId))
            {
                return;
            }

            server.Send(packet, packet.Length, clients[clientId]);
        }

        private bool IsConnected(int clientId)
        {
            if (clients[clientId] == null)
            {
                return false;
            }
            else if (clientLastHeardFrom[clientId].AddMinutes(TIMEOUT_MINUTES) < DateTime.Now)
            {
                // Haven't heard from client in a while
                clients[clientId] = null;
                world.PlayerLeave(clientId);
                Console.WriteLine($"Client #{clientId} timed out");

                return false;
            }

            return true;
        }

        private bool IsValidEndPoint(int clientId, IPEndPoint endPoint)
        {
            return clients[clientId] != null && clients[clientId].Equals(endPoint);
        }

    }
}
