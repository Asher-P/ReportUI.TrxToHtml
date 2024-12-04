using System.Xml.Serialization;
using TrxToHtml.Entities;

namespace TrxToHtml.Helpers;

public static class TrxHelper
{
        /// <summary>
        /// Deserializes a .trx file into a TestReport object.
        /// </summary>
        /// <param name="trxFilePath">The path to the .trx file.</param>
        /// <returns>The deserialized TestReport object.</returns>
        public static TestReport DeserializeTrx(string trxFilePath)
        {
            if (string.IsNullOrWhiteSpace(trxFilePath))
            {
                throw new ArgumentException("The .trx file path cannot be null or empty.", nameof(trxFilePath));
            }

            if (!File.Exists(trxFilePath))
            {
                throw new FileNotFoundException($"The file at '{trxFilePath}' does not exist.");
            }

            try
            {
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(TestReport));
                    using (FileStream fileStream = new FileStream(trxFilePath, FileMode.Open))
                    {
                        return (TestReport)serializer.Deserialize(fileStream);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(
                    "Failed to deserialize the .trx file. Ensure it matches the TestReport schema.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deserializing the .trx file: {ex.Message}", ex);
            }
        }
    }

