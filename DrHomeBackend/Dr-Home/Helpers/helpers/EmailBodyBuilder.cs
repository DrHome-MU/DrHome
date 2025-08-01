﻿namespace Dr_Home.Helpers.helpers
{
    public static class EmailBodyBuilder
    {
        public static string GenerateEmailBody(string template, Dictionary<string, string> templateModel)
        {
            var templatePath = $"{Directory.GetCurrentDirectory()}/Templates/{template}.html";

            var straemReader = new StreamReader(templatePath);
            var body = straemReader.ReadToEnd();

            straemReader.Close();

            foreach (var item in templateModel)
                body = body.Replace(item.Key, item.Value);

            return body;
        }
    }
}
