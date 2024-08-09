using Ozzyria.Gryp.MapTools;
using Ozzyria.Gryp.Models.Data;
using Ozzyria.Gryp.Models;
using Ozzyria.Gryp.Models.Event;

namespace Ozzyria.Gryp.UI.Elements
{
    internal class ToolBeltStrip : ToolStrip, IEventSubscriber<SwitchToDropperEvent>, IEventSubscriber<UnswitchFromDropperEvent>
    {
        internal Map? _map;
        internal ToolBelt _toolBelt = new ToolBelt();

        internal ToolStripButton? _dropperTool = null;
        internal ToolStripButton? _preQuickSwitchTool = null;

        public ToolBeltStrip()
        {
            EventBus.Subscribe(this);
            ItemAdded += OnItemAdded;
        }

        public void AttachMap(Map map)
        {
            _map = map;
        }


        private void OnItemAdded(object? sender, ToolStripItemEventArgs e)
        {
            if (e.Item is ToolStripButton)
            {
                // attach into tool belt handler
                ((ToolStripButton)e.Item).CheckedChanged += OnToolCheckedChanged;
            }
        }

        public void OnToolCheckedChanged(object? sender, EventArgs e)
        {
            if(sender == null)
                return;

            // if is a checked-able tool
            var senderTag = ((ToolStripButton)sender).Tag?.ToString() ?? "";
            ChangeHistory.StartTracking();
            if (senderTag != "entity" && senderTag != "move")
            {
                _map?.UnselectEntity();
            }
            if (senderTag != "wall" && senderTag != "move")
            {
                _map?.UnselectWall();
            }
            ChangeHistory.FinishTracking();
            if (sender is ToolStripButton && ((ToolStripButton)sender).Checked)
            {
                _toolBelt.ToogleTool(senderTag, true);
                foreach (ToolStripItem item in Items)
                {
                    if (item is ToolStripButton && item != sender)
                    {
                        // Uncheck all other tools in the toolbelt
                        _toolBelt.ToogleTool(((ToolStripButton)item).Tag?.ToString() ?? "", false);
                        ((ToolStripButton)item).Checked = false;
                    }
                }
            }
            else if (sender is ToolStripButton)
            {
                _toolBelt.ToogleTool(((ToolStripButton)sender).Tag?.ToString() ?? "", false);
            }
        }

        private ToolStripButton? FindDropper()
        {
            if(_dropperTool != null)
            {
                return _dropperTool;
            }

            foreach (ToolStripItem item in Items)
            {
                if (item is ToolStripButton && (((ToolStripButton)item).Tag?.ToString() ?? "") == "dropper")
                {
                    _dropperTool = (ToolStripButton)item;
                    return _dropperTool;
                }
            }

            return null;
        }

        void IEventSubscriber<SwitchToDropperEvent>.OnNotify(SwitchToDropperEvent e)
        {
            var toolDropper = FindDropper();
            if(toolDropper == null || toolDropper.Checked)
            {
                // no dropper OR dropper is already checked
                return;
            }

            foreach (ToolStripItem item in Items)
            {
                if (item is ToolStripButton && ((ToolStripButton)item).Checked)
                {
                    // track the currently selected tool so it can be reselected on release
                    _preQuickSwitchTool = (ToolStripButton)item;
                }
            }
            toolDropper.Checked = true;
        }

        void IEventSubscriber<UnswitchFromDropperEvent>.OnNotify(UnswitchFromDropperEvent e)
        {
            var toolDropper = FindDropper();
            if (toolDropper != null)
            {
                toolDropper.Checked = false;
            }

            if (_preQuickSwitchTool != null)
            {
                _preQuickSwitchTool.Checked = true;
                _preQuickSwitchTool = null;
            }
        }
    }
}
