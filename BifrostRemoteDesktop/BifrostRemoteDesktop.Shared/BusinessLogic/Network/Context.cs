using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BifrostRemote.Network
{
    public static class Context
    {
        public const int INPUT_TCP_PORT = 65123;
        public const int RECEIVER_BUFFER_SIZE = 1024;
        
        public const char START_OF_TEXT_CHR = '\x02';
        public const char END_OF_TEXT_CHR = '\x03';
    }
}
