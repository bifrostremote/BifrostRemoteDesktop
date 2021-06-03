using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BifrostRemoteDesktop.BusinessLogic.Models.Commands
{
    public abstract class RemoteControlCommandArgs
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}
