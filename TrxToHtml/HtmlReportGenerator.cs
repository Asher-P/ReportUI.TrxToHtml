using System.Text;
using TrxToHtml.Entities;
using TrxToHtml.Helpers;

namespace TrxToHtml;

public static class HtmlReportGenerator
{
    public static string GenerateHtmlReport(TestReport testReport)
    {
        StringBuilder html = new StringBuilder();

        // Start of HTML document with internal CSS for styling
        html.Append("<html><head>");
        html.Append("<title>Test Report</title>");
        html.Append("<style>");
        html.Append("body { font-family: Arial, sans-serif; background-color: #f4f7fa; margin: 0; padding: 20px; }");
        html.Append("h1, h2, h3 { color: #333; }");
        html.Append("h1 { border-bottom: 2px solid #007bff; padding-bottom: 10px; margin-bottom: 20px; }");

        // Ensure table fits the screen
        html.Append("table { width: 100%; max-width: 100%; border-collapse: collapse; margin: 20px 0; overflow-x: auto; display: block; box-sizing: border-box; }");
        html.Append("th, td { padding: 12px 20px; text-align: left; border: 1px solid #ddd; vertical-align: top; }");
        html.Append("th { background-color: #007bff; color: white; font-size: 18px; }");
        html.Append("td { background-color: #ffffff; color: #333; font-size: 16px; word-wrap: break-word; white-space: pre-wrap; }");
        html.Append("tr:nth-child(even) td { background-color: #f9f9f9; }");
        html.Append("tr:hover { background-color: #e9f5ff; }");

        // Outcome-specific styling
        html.Append("tr td.pass { background-color: #28a745 !important; color: white; }");
        html.Append("tr td.fail { background-color: #dc3545 !important; color: white; }");
        html.Append("tr td.skip { background-color: #ffc107 !important; color: white; }");

        // Responsive Design
        html.Append("@media (max-width: 768px) {");
        html.Append("table { display: block; overflow-x: auto; }");
        html.Append("th, td { font-size: 14px; padding: 8px; }");
        html.Append("}");

        // Expandable StdOut
        html.Append(".stdout-content { max-height: 60px; overflow: hidden; text-overflow: ellipsis; white-space: pre-wrap; position: relative; }");
        html.Append(".stdout-content.expanded { max-height: none; white-space: pre-wrap; }");
        html.Append(".toggle-btn { background-color: #007bff; color: white; border: none; padding: 5px 10px; margin-top: 5px; cursor: pointer; border-radius: 3px; font-size: 14px; display: inline-block; }");
        html.Append(".toggle-btn:hover { background-color: #0056b3; }");

        html.Append("</style>");
        html.Append("<script>");
        html.Append("function toggleStdOut(id) {");
        html.Append("var content = document.getElementById('stdout-' + id);");
        html.Append("var button = document.getElementById('toggle-' + id);");
        html.Append("if (content.classList.contains('expanded')) {");
        html.Append("content.classList.remove('expanded');");
        html.Append("button.innerHTML = 'Show More';");
        html.Append("} else {");
        html.Append("content.classList.add('expanded');");
        html.Append("button.innerHTML = 'Show Less';");
        html.Append("}");
        html.Append("}");
        html.Append("</script>");
        html.Append("</head><body>");

        // Report Header
        html.Append("<h1>Test Report for: " + testReport.Name + "</h1>");
        html.Append("<p><strong>Test Run ID:</strong> " + testReport.Id + "</p>");
        html.Append("<p><strong>Created On:</strong> " + testReport.Times.Creation.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
        html.Append("<p><strong>Start Time:</strong> " + testReport.Times.Start.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
        html.Append("<p><strong>Finish Time:</strong> " + testReport.Times.Finish.ToString("yyyy-MM-dd HH:mm:ss") + "</p>");
        
        // Test Results Section
        html.Append("<div class='section-title'>Test Results</div>");
        html.Append("<table>");
        html.Append("<tr><th>Test Name</th><th>Outcome</th><th>Computer</th><th>StdOut</th></tr>");

        int stdoutId = 0;
        foreach (var result in testReport.Results)
        {
            // Ensure Outcome has a valid value
            string outcome = result.Outcome?.Trim() ?? "Unknown";
            string outcomeClass = outcome.ToLower() switch
            {
                "passed" => "pass",
                "failed" => "fail",
                "skipped" => "skip",
                _ => "fail" // Default to "fail" styling for unknown outcomes
            };

            html.Append("<tr>");
            html.Append("<td>" + result.TestName + "</td>");
            html.Append($"<td class='{outcomeClass}'>" + outcome + "</td>"); // Assign outcome class directly
            html.Append("<td>" + (result.ComputerName ?? "Unknown Computer") + "</td>");
            html.Append("<td>");
            html.Append("<div id='stdout-" + stdoutId + "' class='stdout-content'>" +
                        (string.IsNullOrEmpty(result.Output?.StdOut) ? "No Output" : result.Output.StdOut) +
                        "</div>");
            html.Append("<button id='toggle-" + stdoutId + "' class='toggle-btn' onclick='toggleStdOut(" + stdoutId + ")'>Show More</button>");
            html.Append("</td>");
            html.Append("</tr>");
            stdoutId++;
        }

        html.Append("</table>");

        // Result Summary Section
        if (testReport.ResultSummary != null)
        {
            html.Append("<div class='summary'>");
            html.Append("<h3>Result Summary</h3>");
            html.Append("<p><strong>Outcome:</strong> " + testReport.ResultSummary.Outcome + "</p>");
            if (testReport.ResultSummary.Counters != null)
            {
                html.Append("<p><strong>Total Tests:</strong> " + testReport.ResultSummary.Counters.Total + "</p>");
                html.Append("<p><strong>Executed Tests:</strong> " + testReport.ResultSummary.Counters.Executed + "</p>");
                html.Append("<p><strong>Passed Tests:</strong> " + testReport.ResultSummary.Counters.Passed + "</p>");
                html.Append("<p><strong>Failed Tests:</strong> " + testReport.ResultSummary.Counters.Failed + "</p>");
            }
            html.Append("</div>");
        }

        // End of HTML document
        html.Append("</body></html>");

        return html.ToString();
    }

    // Save method
    public static void SaveReportToFile(string trxReportPath,string savedHtmlPath)
    {
        try
        {
            var testReport = TrxHelper.DeserializeTrx(trxReportPath);
            string htmlContent = GenerateHtmlReport(testReport);
            File.WriteAllText(savedHtmlPath, htmlContent);
            Console.WriteLine("Report saved successfully at: " + savedHtmlPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving report: " + ex.Message);
        }
    }
}