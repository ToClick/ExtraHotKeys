
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ExtraHotKeys
{
    internal class Config
    {
        private string _filepath;
        private List<KeyAction> _actions;

        public Config()
        {
            string dir = new FileInfo(Application.ExecutablePath).DirectoryName;
            _filepath = Path.Combine(dir, "config.json");
            _actions = new List<KeyAction>();

            if (File.Exists(_filepath))
            {
                var json = File.ReadAllText(_filepath);
                _actions = new JavaScriptSerializer().Deserialize<List<KeyAction>>(json);
            }
        }

        public KeyAction Get(int id)
        {
            var action = _actions.Find(x => x.id == id);
            action.id = id;

            return action;
        }

        public List<KeyAction> GetAll()
        {
            return _actions;
        }

        public void Set(KeyAction action)
        {
            for (var i = 0; i < _actions.Count; i++)  {

                if (_actions[i].id != action.id) {
                    continue;
                }


                if (action.app == _actions[i].app &&
                    action.enable == _actions[i].enable &&
                    _actions[i].keys != null &&
                    action.keys.SequenceEqual(_actions[i].keys))
                {
                    return;
                }


                _actions[i] = action;
                Save();
                return;
            }

            _actions.Add(action);
            Save();
        }

        public void SetKeys(int id, VKeys[] keys)
        {
            var action = Get(id);
            action.keys = keys.Cast<short>().ToArray();

            Set(action);
        }

        public void SetApp(int id, string app)
        {
            var action = Get(id);
            action.app = app;

            Set(action);
        }

        public void SetEnable(int id, bool enable)
        {
            var action = Get(id);
            action.enable = enable;

            Set(action);
        }

        public void Save()
        {
            Hook.Actions = _actions;

            var json = new JavaScriptSerializer().Serialize(_actions);
            File.WriteAllText(_filepath, json);
        }
    }
}
