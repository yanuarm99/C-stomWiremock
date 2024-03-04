namespace CustomWireMock
{
    internal class EnvironmentVariable
    {        
        public static string GetEnvironmentVariable(string variableName)
        {
            string variableValue = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(variableValue))
            {
                variableValue = Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
            }

            return variableValue;
        }
    }
}