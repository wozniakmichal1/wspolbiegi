using System.Windows.Input;

namespace PresentationViewModel.MVVMLight
{
    public class RelayCommand : ICommand
    {
        private readonly Action m_Execute;
        private readonly Func<bool> m_CanExecute;

        public RelayCommand(Action action) : this(action, null) { }

        public RelayCommand(Action action, Func<bool> canExecute)
        {
            m_CanExecute = canExecute;
            m_Execute = action;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (this.m_CanExecute == null)
            {
                return true;
            }
            return this.m_CanExecute();
        }

        public void Execute(object? parameter)
        {
            this.m_Execute();
        }
    }
}
