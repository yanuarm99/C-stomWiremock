using WireMock.Server;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using System;
using System.IO;
using System.Text.RegularExpressions;
using WireMock.Settings;

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