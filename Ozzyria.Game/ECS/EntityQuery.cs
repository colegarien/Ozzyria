using System;
using System.Collections.Generic;

namespace Ozzyria.Game.ECS
{
    public class EntityQuery
    {
        public List<Type> ands = new List<Type>();
        public List<Type> ors = new List<Type>();
        public List<Type> nones = new List<Type>();

        public bool IsEmpty()
        {
            return ands.Count == 0
                && ors.Count == 0;
        }

        /// <summary>
        /// Add criteria that all entities must meet
        /// </summary>
        /// <param name="types">Component Types to add to filter</param>
        /// <returns>this to allow for chaining</returns>
        public EntityQuery And(params Type[] types)
        {
            ands.AddRange(types);
            return this;
        }

        /// <summary>
        /// Add criteria that all entities must have at-least one of
        /// </summary>
        /// <param name="types">Component Types to add to filter</param>
        /// <returns>this to allow for chaining</returns>
        public EntityQuery Or(params Type[] types)
        {
            ors.AddRange(types);
            return this;
        }

        /// <summary>
        /// Filter out entities with specific components
        /// Must be used with AND/OR query
        /// </summary>
        /// <param name="types">Component Types to add to filter</param>
        /// <returns>this to allow for chaining</returns>
        public EntityQuery None(params Type[] types)
        {
            nones.AddRange(types);
            return this;
        }
    }
}
