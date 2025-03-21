using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DartsMobilApp.Classes
{
    using CommunityToolkit.Mvvm.Messaging.Messages;

    public class SelectedMatchMessage : ValueChangedMessage<MatchModel>
    {
        public SelectedMatchMessage(MatchModel match) : base(match) { }
    }

}
