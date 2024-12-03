namespace TransStarter_Task.WPFApplication.API
{
    public class LoadingController
    {
        private float _max { get; set; }
        private float _current { get; set; }
        private string _operationName { get; set; } = string.Empty;

        public void SetMax (float newMax)
        {
            _max = newMax;
            OnStateChanged.Invoke(CalculatePercent());
        }

        public void SetCurrent (float newCurrent)
        {
            _current = newCurrent;
            OnStateChanged.Invoke(CalculatePercent());
        }

        public void SetOperationName (string newOperationName)
        {
            _operationName = newOperationName;
            OperationNameChanged.Invoke(_operationName);
        }

        private float CalculatePercent () => _current / _max;

        public void Complete () =>
            OperationCompleted.Invoke();

        /// <summary>
        /// Передаваемой значение - это процент от заполнения (Current/Max)
        /// </summary>
        public event Action<float> OnStateChanged = x => {};

        public event Action<string> OperationNameChanged  = x => {};

        public event Action OperationCompleted = () => {};
    }
}
