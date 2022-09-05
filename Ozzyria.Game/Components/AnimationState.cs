using Ozzyria.Game.Components.Attribute;
using Ozzyria.Game.ECS;
using System.Collections.Generic;
using System.Text;

namespace Ozzyria.Game.Components
{
    public class AnimationState : Component
    {
        private string _state = "idle";

        private IDictionary<string, string> _variables = new Dictionary<string, string>();
        private string _encodeCache = "";
        private bool _needsEncoded = true;

        [Savable]
        public string State
        {
            get => _state; set
            {
                if (_state != value)
                {
                    _state = value;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        [Savable]
        public string EncodedVariables
        {
            get {
                if (_needsEncoded)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var kv in _variables)
                    {
                        builder.Append($"{kv.Key}={kv.Value}|");
                    }
                    _encodeCache = builder.ToString();

                    _needsEncoded = false;
                }
                return _encodeCache;
            }
            set
            {
                _variables.Clear();
                var variables = value.Split("|");
                foreach(var variableStatement in variables)
                {
                    var pieces = variableStatement.Split('=');
                    if (pieces.Length != 2)
                        continue;

                    _variables.Add(pieces[0], pieces[1]);
                }
                OnComponentChanged?.Invoke(Owner, this);
            }
        }

        public IDictionary<string, string> Variables
        {
            get => _variables; set
            {
                if (_variables != value)
                {
                    _variables = value;
                    _needsEncoded = true;
                    OnComponentChanged?.Invoke(Owner, this);
                }
            }
        }

        public string GetVariable(string name)
        {
            return _variables.ContainsKey(name) ? _variables[name] : "";
        }

        public void SetVariable(string name, string value)
        {
            string originalValue = _variables.ContainsKey(name) ? _variables[name] : null;
            _variables[name] = value;

            if (originalValue == null || originalValue != value)
            {
                _needsEncoded = true;
                OnComponentChanged?.Invoke(Owner, this);
            }
        }

        ///
        /// HELPER Functions
        ///
        public bool GetBoolVariable(string name)
        {
            return GetVariable(name).Equals("true");
        }

        public string GetDirectionVariable(string name)
        {
            var value = GetVariable(name);
            return value != "" && (value == "east" || value == "west" || value == "south" || value == "north")
                ? value
                : "east";
        }
    }
}
