namespace Anvil
{
    public static partial class ExtensionMethods
    {
        public static ITrajectoryCalculator CreateSafe(this ITrajectoryParameter<ITrajectoryCalculator> parameter)
        {
            if (parameter == null)
            {
                return LinearTrajectoryCalculator.Instance;
            }
            var calculator = parameter.CreateTrajectoryCalculator();
            if (calculator == null)
            {
                return LinearTrajectoryCalculator.Instance;
            }
            return calculator;
        }
    }
}