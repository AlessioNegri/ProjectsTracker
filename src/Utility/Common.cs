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
        
        /// <summary> Checks if the new version is greater than the old one </summary>
        /// <param name="new_version"> New version number </param>
        /// <param name="old_version"> Old version number </param>
        /// <returns> True if the check was a success </returns>
        public static bool CheckVersion(in string new_version, in string old_version)
        {
            if (new_version == old_version) return true;

            if (string.IsNullOrEmpty(new_version) || string.IsNullOrEmpty(old_version)) return true;

            if (new_version.Split('.').Length > 0 && old_version.Split('.').Length > 0)
            {
                if (Int32.Parse(new_version.Split('.')[0]) < Int32.Parse(old_version.Split('.')[0])) return false;
                if (Int32.Parse(new_version.Split('.')[0]) > Int32.Parse(old_version.Split('.')[0])) return true;
            }

            if (new_version.Split('.').Length > 1 && old_version.Split('.').Length > 1)
            {
                if (Int32.Parse(new_version.Split('.')[1]) < Int32.Parse(old_version.Split('.')[1])) return false;
                if (Int32.Parse(new_version.Split('.')[1]) > Int32.Parse(old_version.Split('.')[1])) return true;
            }

            if (new_version.Split('.').Length > 2 && old_version.Split('.').Length > 2)
            {
                if (Int32.Parse(new_version.Split('.')[2]) < Int32.Parse(old_version.Split('.')[2])) return false;
                if (Int32.Parse(new_version.Split('.')[2]) > Int32.Parse(old_version.Split('.')[2])) return true;
            }

            if (new_version.Split('.').Length > 3 && old_version.Split('.').Length > 3)
            {
                if (Int32.Parse(new_version.Split('.')[3]) < Int32.Parse(old_version.Split('.')[3])) return false;
                if (Int32.Parse(new_version.Split('.')[3]) > Int32.Parse(old_version.Split('.')[3])) return true;
            }

            return true;
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

        /// <summary> Parses the date to format it correctly </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ParseDate(string date)
        {
            if (date == "0000-00-00" || date == "") return date;

            DateTime formatted_date = new DateTime();

            if (DateTime.TryParseExact(date, "M/d/yyyy hh:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out formatted_date))
            {
                return formatted_date.ToString("yyyy-MM-dd");
            }

            if (DateTime.TryParseExact(date, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out formatted_date))
            {
                return formatted_date.ToString("yyyy-MM-dd");
            }

            return date;
        }
    }
}