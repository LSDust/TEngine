namespace TEngine
{
    public class InputModule : Module,IInputModule
    {
        public override int Priority => 8;
        public InputSystem_Actions.PlayerActions Player => Control.Player;
        public InputSystem_Actions.UIActions UI => Control.UI;
        private InputSystem_Actions Control => _control ??= new InputSystem_Actions();
        private InputSystem_Actions _control;
        
        public override void OnInit()
        {
            Control.Enable();
        }

        public override void Shutdown()
        {
            Control.Disable();
        }
    }
}
