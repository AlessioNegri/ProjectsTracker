namespace ProjectsTracker.src.Utility
{
    /// <summary> Common functions </summary>
    public static class Common
    {
        /// <summary> Prepare the version for DB </summary>
        /// <param name="version"> Version (e.g. 1.2.3) </param>
        /// <returns> DB version (e.g. 00001.00002.00005) </returns>
        public static string PrepareVersion(string version)
        {
            if (string.IsNullOrEmpty(version)) return string.Empty;

            var list = version.Split('.');

            var preparedVersion = string.Empty;

            foreach (var e in list) preparedVersion += e.PadLeft(5, '0') + '.';

            preparedVersion = preparedVersion.Remove(preparedVersion.Length - 1, 1);

            return preparedVersion;
        }

        /// <summary> Converts the version from DB </summary>
        /// <param name="version"> Version (e.g. 00001.00002.00005) </param>
        /// <returns> DB version (e.g. 1.2.3) </returns>
        public static string ConvertVersion(string version)
        {
            if (string.IsNullOrEmpty(version)) return string.Empty;

            var list = version.Split('.');

            var convertedVersion = string.Empty;

            foreach (var e in list) convertedVersion += Int32.Parse(e).ToString() + '.';

            convertedVersion = convertedVersion.Remove(convertedVersion.Length - 1, 1);

            return convertedVersion;
        }

        /// <summary> Encodes in a UNICODE string </summary>
        /// <param name="value"> Not encoded string </param>
        /// <returns> Encoded string </returns>
        public static string EncodeUnicode(string value)
        {
            value = value.Replace("\"", "&#34;");
            value = value.Replace("\'", "&#39;");
            value = value.Replace("`", "&#44;");

            return value;
        }

        /// <summary> Decodes a string from UNICODE </summary>
        /// <param name="value"> Encoded string </param>
        /// <returns> Not encoded string </returns>
        public static string DecodeUnicode(string value)
        {
            value = value.Replace("&#34;", "\"");
            value = value.Replace("&#39;", "\'");
            value = value.Replace("&#44;", "`");

            return value;
        }
    }
}