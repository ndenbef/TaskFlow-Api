﻿namespace TaskFlowProjectApi.Data
{
    public class CreateProject
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
    }
}
