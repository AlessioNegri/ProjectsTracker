using Newtonsoft.Json;
using ProjectsTracker.src.Utility;
using System.Text.Json;

namespace ProjectsTracker.src.Database
{
    /// <summary> Type of row </summary>
    enum RowType : int
    {
        ECR     = 0,
        PR      = 1,
        RELEASE = 2,
        PATCH   = 3
    }

    /// <summary> Class to manage "project_{id}" SQL requests </summary>
    class TablesManager
    {
        #region MEMBERS

        /// <summary> Singleton instance </summary>
        private static TablesManager? instance = null;

        /// <summary> Thread lock </summary>
        private static readonly object padlock = new object();

        #endregion

        #region METHODS - PUBLIC

        /// <summary> Retrieves the class instance </summary>
        public static TablesManager Instance
        {
            get { lock (padlock) { if (instance is null) instance = new TablesManager(); return instance; } }
        }

        /// <summary> Executes a SELECT query on "project_{id}" table </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="version"> Filtered version </param>
        /// <param name="type"> Filtered type </param>
        /// <param name="status"> Filtered status </param>
        /// <param name="priority"> Filtered priority </param>
        /// <param name="elements"> List of rows </param>
        /// <returns> Success of the operation </returns>
        public bool SelectRows(in int project_id, in string version, in int type, in int status, in int priority, out List<ROW_TABLE> elements)
        {
            // Get last version

            var versions = new List<string>();

            SelectVersions(project_id, out versions);

            var lastVersion = (versions.Count > 0) ? versions[versions.Count - 1] : "";

            // Filters

            var conditions = new List<string>();

            if (version != "All")
            {
                if (version == lastVersion)
                {
                    conditions.Add($"Version IN ('{Common.PrepareVersion(version)}', '')");
                }
                else
                {
                    conditions.Add($"Version = '{Common.PrepareVersion(version)}'");
                }
            }

            if (type != -1) conditions.Add($"Type = {type}");

            if (status != -1) conditions.Add($"Status = {status}");

            if (priority != -1) conditions.Add($"Priority = {priority}");

            var condition = string.Empty;

            if (conditions.Count > 0) condition = "WHERE " + String.Join(" AND ", conditions);

            // Query

            string query = $"SELECT * FROM project_{project_id} {condition} ORDER BY Version, Type, Number;";

            // Execution

            elements = new List<ROW_TABLE>();

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_TABLE>? jsonArr = JsonConvert.DeserializeObject<List<ROW_TABLE>>(json);

            if (jsonArr is null) return false;

            foreach (var obj in jsonArr)
            {
                ROW_TABLE row = new ROW_TABLE();

                row.ID                  = obj.ID;
                row.CreationDate        = obj.CreationDate;
                row.UpdateDate          = obj.UpdateDate;
                row.ClosureDate         = obj.ClosureDate;
                row.Version             = Common.ConvertVersion(obj.Version);
                row.PatchVersion        = Common.ConvertVersion(obj.PatchVersion);
                row.ReferenceVersion    = Common.ConvertVersion(obj.ReferenceVersion);
                row.Type                = obj.Type;
                row.Number              = obj.Number;
                row.Status              = obj.Status;
                row.Priority            = obj.Priority;
                row.Title               = Common.DecodeUnicode(obj.Title);
                row.Description         = Common.DecodeUnicode(obj.Description);
                row.Note                = Common.DecodeUnicode(obj.Note);
                row.Ordering            = row.Version.ToString() + "&" + (Common.CheckVersion(row.Version, row.PatchVersion) ? "" : row.PatchVersion.ToString());

                elements.Add(row);
            }

            return true;
        }

        /// <summary> Executes a SELECT query on "project_{id}" table for the given Id </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row_id"> Row Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool SelectRowById(in int project_id, in int row_id, out ROW_TABLE row)
        {
            row = new ROW_TABLE();

            string query = $"SELECT * FROM project_{project_id} WHERE ID = {row_id};";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_TABLE>? jsonArr = JsonConvert.DeserializeObject<List<ROW_TABLE>>(json);

            if (jsonArr is null) return false;

            if (jsonArr.Count == 0) return false;

            row = jsonArr.ElementAt(0);

            row.Version             = Common.ConvertVersion(row.Version);
            row.PatchVersion        = Common.ConvertVersion(row.PatchVersion);
            row.ReferenceVersion    = Common.ConvertVersion(row.ReferenceVersion);
            row.Title               = Common.DecodeUnicode(row.Title);
            row.Description         = Common.DecodeUnicode(row.Description);
            row.Note                = Common.DecodeUnicode(row.Note);

            return true;
        }

        /// <summary> Executes a SELECT query on "project_{id}" table for the available versions </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="versions"> List of versions </param>
        /// <returns> Success of the operation </returns>
        public bool SelectVersions(in int project_id, out List<string> versions)
        {
            versions = new List<string>();

            string query = $"SELECT * FROM project_{project_id} GROUP BY Version ORDER BY Version;";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            List<ROW_TABLE>? jsonArr = JsonConvert.DeserializeObject<List<ROW_TABLE>>(json);

            if (jsonArr is null) return false;

            if (jsonArr.Count == 0) return false;

            foreach (var obj in jsonArr)
            {
                versions.Add(Common.ConvertVersion(obj.Version));
            }

            return true;
        }


        /// <summary> Inserts a new ECR </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool InsertECR(in int project_id, ROW_TABLE row)
        {
            // Calculate new number

            int number = 0;

            if (!CalculateNextNumber(project_id, RowType.ECR, out number)) return false;

            // Insert ECR

            var query =
                $"INSERT INTO project_{project_id} " +
                $"(CreationDate, UpdateDate, ClosureDate, Version, PatchVersion," +
                $"Type, Number, Status, Priority, Title, Description) " +
                $"VALUES " +
                $"(@creationDate, @updateDate, @closureDate, @version, @patchVersion, " +
                $"@type, @number, @status, @priority, @title, @description)";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@patchVersion", Common.PrepareVersion(row.PatchVersion));
            parameters.Add("@type", ((int)RowType.ECR));
            parameters.Add("@number", number);
            parameters.Add("@status", row.Status);
            parameters.Add("@priority", row.Priority);
            parameters.Add("@title", Common.EncodeUnicode(row.Title));
            parameters.Add("@description", Common.EncodeUnicode(row.Description));

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Updates an existing ECR </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool UpdateECR(in int project_id, ROW_TABLE row)
        {
            string query = 
                $"UPDATE project_{project_id} SET " +
                $"CreationDate = @creationDate, " +
                $"UpdateDate = @updateDate, " +
                $"ClosureDate = @closureDate, " +
                $"Version = @version, " +
                $"PatchVersion = @patchVersion, " +
                $"Status = @status, " +
                $"Priority = @priority, " +
                $"Title = @title, " +
                $"Description = @description " +
                $"WHERE ID = @id;";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@patchVersion", Common.PrepareVersion(row.PatchVersion));
            parameters.Add("@status", row.Status);
            parameters.Add("@priority", row.Priority);
            parameters.Add("@title", Common.EncodeUnicode(row.Title));
            parameters.Add("@description", Common.EncodeUnicode(row.Description));
            parameters.Add("@id", row.ID);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Inserts a new PR </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool InsertPR(in int project_id, ROW_TABLE row)
        {
            // Calculate new number

            int number = 0;

            if (!CalculateNextNumber(project_id, RowType.PR, out number)) return false;

            // Insert ECR

            var query =
                $"INSERT INTO project_{project_id} " +
                $"(CreationDate, UpdateDate, ClosureDate, Version, PatchVersion, ReferenceVersion, " +
                $"Type, Number, Status, Priority, Title, Description, Note) " +
                $"VALUES " +
                $"(@creationDate, @updateDate, @closureDate, @version, @patchVersion, @referenceVersion, " +
                $"@type, @number, @status, @priority, @title, @description, @note)";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@patchVersion", Common.PrepareVersion(row.PatchVersion));
            parameters.Add("@referenceVersion", Common.PrepareVersion(row.ReferenceVersion));
            parameters.Add("@type", ((int)RowType.PR));
            parameters.Add("@number", number);
            parameters.Add("@status", row.Status);
            parameters.Add("@priority", row.Priority);
            parameters.Add("@title", Common.EncodeUnicode(row.Title));
            parameters.Add("@description", Common.EncodeUnicode(row.Description));
            parameters.Add("@note", Common.EncodeUnicode(row.Note));

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Updates an existing PR </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool UpdatePR(in int project_id, ROW_TABLE row)
        {
            string query = 
                $"UPDATE project_{project_id} SET " +
                $"CreationDate = @creationDate, " +
                $"UpdateDate = @updateDate, " +
                $"ClosureDate = @closureDate, " +
                $"Version = @version, " +
                $"PatchVersion = @patchVersion, " +
                $"ReferenceVersion = @referenceVersion, " +
                $"Status = @status, " +
                $"Priority = @priority, " +
                $"Title = @title, " +
                $"Description = @description, " +
                $"Note = @note " +
                $"WHERE ID = @id;";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@patchVersion", Common.PrepareVersion(row.PatchVersion));
            parameters.Add("@referenceVersion", Common.PrepareVersion(row.ReferenceVersion));
            parameters.Add("@status", row.Status);
            parameters.Add("@priority", row.Priority);
            parameters.Add("@title", Common.EncodeUnicode(row.Title));
            parameters.Add("@description", Common.EncodeUnicode(row.Description));
            parameters.Add("@note", Common.EncodeUnicode(row.Note));
            parameters.Add("@id", row.ID);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Inserts a new RELEASE </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool InsertRELEASE(in int project_id, ROW_TABLE row)
        {
            // Calculate new number

            int number = 0;

            if (!CalculateNextNumber(project_id, RowType.RELEASE, out number)) return false;

            // Insert ECR

            var query =
                $"INSERT INTO project_{project_id} " +
                $"(CreationDate, UpdateDate, ClosureDate, Version, Type, Number, Status) " +
                $"VALUES " +
                $"(@creationDate, @updateDate, @closureDate, @version, @type, @number, @status)";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@type", ((int)RowType.RELEASE));
            parameters.Add("@number", number);
            parameters.Add("@status", row.Status);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Updates an existing RELEASE </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool UpdateRELEASE(in int project_id, ROW_TABLE row)
        {
            string query = 
                $"UPDATE project_{project_id} SET " +
                $"CreationDate = @creationDate, " +
                $"UpdateDate = @updateDate, " +
                $"ClosureDate = @closureDate, " +
                $"Version = @version, " +
                $"Status = @status " +
                $"WHERE ID = @id;";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@status", row.Status);
            parameters.Add("@id", row.ID);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Inserts a new PATCH </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool InsertPATCH(in int project_id, ROW_TABLE row)
        {
            // Calculate new number

            int number = 0;

            if (!CalculateNextNumber(project_id, RowType.PATCH, out number)) return false;

            // Insert ECR

            var query =
                $"INSERT INTO project_{project_id} " +
                $"(CreationDate, UpdateDate, ClosureDate, Version, PatchVersion, Type, Number, Status) " +
                $"VALUES " +
                $"(@creationDate, @updateDate, @closureDate, @version, @patchVersion, @type, @number, @status)";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@patchVersion", Common.PrepareVersion(row.PatchVersion));
            parameters.Add("@type", ((int)RowType.PATCH));
            parameters.Add("@number", number);
            parameters.Add("@status", row.Status);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Updates an existing PATCH </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row"> Row </param>
        /// <returns> Success of the operation </returns>
        public bool UpdatePATCH(in int project_id, ROW_TABLE row)
        {
            string query = 
                $"UPDATE project_{project_id} SET " +
                $"CreationDate = @creationDate, " +
                $"UpdateDate = @updateDate, " +
                $"ClosureDate = @closureDate, " +
                $"Version = @version, " +
                $"PatchVersion = @patchVersion, " +
                $"Status = @status " +
                $"WHERE ID = @id;";

            Dictionary<String, Object> parameters = new Dictionary<String, Object>();

            parameters.Add("@creationDate", row.CreationDate);
            parameters.Add("@updateDate", DateTime.Now.ToString(format: "yyyy-MM-dd HH.mm.ss"));
            parameters.Add("@closureDate", row.ClosureDate);
            parameters.Add("@version", Common.PrepareVersion(row.Version));
            parameters.Add("@patchVersion", Common.PrepareVersion(row.PatchVersion));
            parameters.Add("@status", row.Status);
            parameters.Add("@id", row.ID);

            if (!DBMS.Instance.ExecuteQuery(query, parameters)) return false;

            return true;
        }

        /// <summary> Deletes an existing row </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row_id"> Row Id </param>
        /// <returns> Success of the operation </returns>
        public bool DeleteRow(in int project_id, in int row_id)
        {
            var query = $"DELETE FROM project_{project_id} WHERE ID = {row_id}";

            if (!DBMS.Instance.ExecuteQuery(query)) return false;

            return true;
        }

        #endregion

        #region METHODS - PRIVATE

        /// <summary> Constructor </summary>
        private TablesManager() { }


        /// <summary> Calculates the new number of a project's row </summary>
        /// <param name="project_id"> Project Id </param>
        /// <param name="row_type"> Row Type (ECR - PR - RELEASE - PATCH) </param>
        /// <returns></returns>
        private bool CalculateNextNumber(in int project_id, in RowType row_type, out int number)
        {
            number = 0;

            string query = $"SELECT Max(Number) FROM project_{project_id} WHERE Type = {((int)row_type)};";

            string json = string.Empty;

            if (!DBMS.Instance.ExecuteReader(query, out json)) return false;

            using JsonDocument doc = JsonDocument.Parse(json);

            number = doc.RootElement.EnumerateArray().ElementAt(0).GetProperty("Max(Number)").GetInt32() + 1;

            return true;
        }

        #endregion
    }
}