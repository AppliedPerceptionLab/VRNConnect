using System.Text;

public class Task
{
    public int taskType;
    public string interactionMethod;
    public string [] node;
    public string detailToRead;
    public bool taskFinish = false;
    public long taskTime = 0;
    public int taskClicks = 0;

    internal string GetTooltipText()
    {
        var sb = new StringBuilder();
        if (taskType == 1)
        {
            sb.AppendLine("Task1: Node Selection");
            sb.AppendLine($"Select the colorized node using your {interactionMethod}");
            sb.AppendLine($"Then read node's \"{detailToRead}\"");
        } else if (taskType == 2)
        {
            sb.AppendLine("Task2: Shortest Path");
            sb.AppendLine($"Select the 2 colorized nodes using your {interactionMethod}");
            sb.AppendLine($"Then \"{detailToRead}\"");
        }
        
        return sb.ToString();
    }
}
