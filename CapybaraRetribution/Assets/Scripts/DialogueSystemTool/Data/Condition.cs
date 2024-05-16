namespace DialogueSystemTool.Data
{
    /// <summary>
    /// Represents a condition in the dialogue system.
    /// </summary>
    [System.Serializable]
    public class Condition
    {
        public string variableName; // Name of the variable to check
        public string expectedValue; // Expected value of the variable
    }
}