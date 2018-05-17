using System;

namespace GUIProject.Tcp
{
    class ClientMessage : EventArgs
    {
        private string message;

        public string Message { get { return this.message; } }

        public ClientMessage(string message)
        {
            this.message = message;
        }
    }
}