using System.ComponentModel;

namespace Celerik.NetCore.Services
{
    /// <summary>
    /// Defines the possible types of operation that the API can execute.
    /// </summary>
    public enum ApiOperationType
    {
        /// <summary>
        /// A read operation that does not alter any data.
        /// </summary>
        [Description("Read")]
        Read = 1,

        /// <summary>
        /// An operation that inserts a single record.
        /// </summary>
        [Description("Insert")]
        Insert = 2,

        /// <summary>
        /// An operation that inserts several records.
        /// </summary>
        [Description("Bulk Insert")]
        BulkInsert = 3,

        /// <summary>
        /// An operation that updates a single record.
        /// </summary>
        [Description("Update")]
        Update = 4,

        /// <summary>
        /// An operation that updates several records.
        /// </summary>
        [Description("Bulk Update")]
        BulkUpdate = 5,

        /// <summary>
        /// An operation that deletes a single record.
        /// </summary>
        [Description("Delete")]
        Delete = 6,

        /// <summary>
        /// An operation that deletes several records.
        /// </summary>
        [Description("Bulk Delete")]
        BulkDelete = 7
    }
}
