using SQLitePCL;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SqliteVirtualTables;

public static unsafe class Constants
{
    /*
    ** CAPI3REF: Result Codes
    ** KEYWORDS: {result code definitions}
    **
    ** Many SQLite functions return an integer result code from the set shown
    ** here in order to indicate success or failure.
    **
    ** New error codes may be added in future versions of SQLite.
    **
    ** See also: [extended result code definitions]
    */
    public const int SQLITE_OK = 0;   /* Successful result */
    /* beginning-of-error-codes */
    public const int SQLITE_ERROR = 1;   /* Generic error */
    public const int SQLITE_INTERNAL = 2;   /* Internal logic error in SQLite */
    public const int SQLITE_PERM = 3;   /* Access permission denied */
    public const int SQLITE_ABORT = 4;   /* Callback routine requested an abort */
    public const int SQLITE_BUSY = 5;   /* The database file is locked */
    public const int SQLITE_LOCKED = 6;   /* A table in the database is locked */
    public const int SQLITE_NOMEM = 7;   /* A malloc() failed */
    public const int SQLITE_READONLY = 8;   /* Attempt to write a readonly database */
    public const int SQLITE_INTERRUPT = 9;   /* Operation terminated by sqlite3_interrupt()*/
    public const int SQLITE_IOERR = 10;   /* Some kind of disk I/O error occurred */
    public const int SQLITE_CORRUPT = 11;   /* The database disk image is malformed */
    public const int SQLITE_NOTFOUND = 12;   /* Unknown opcode in sqlite3_file_control() */
    public const int SQLITE_FULL = 13;   /* Insertion failed because database is full */
    public const int SQLITE_CANTOPEN = 14;   /* Unable to open the database file */
    public const int SQLITE_PROTOCOL = 15;   /* Database lock protocol error */
    public const int SQLITE_EMPTY = 16;   /* Internal use only */
    public const int SQLITE_SCHEMA = 17;   /* The database schema changed */
    public const int SQLITE_TOOBIG = 18;   /* String or BLOB exceeds size limit */
    public const int SQLITE_CONSTRAINT = 19;   /* Abort due to constraint violation */
    public const int SQLITE_MISMATCH = 20;   /* Data type mismatch */
    public const int SQLITE_MISUSE = 21;   /* Library used incorrectly */
    public const int SQLITE_NOLFS = 22;   /* Uses OS features not supported on host */
    public const int SQLITE_AUTH = 23;   /* Authorization denied */
    public const int SQLITE_FORMAT = 24;   /* Not used */
    public const int SQLITE_RANGE = 25;   /* 2nd parameter to sqlite3_bind out of range */
    public const int SQLITE_NOTADB = 26;   /* File opened that is not a database file */
    public const int SQLITE_NOTICE = 27;   /* Notifications from sqlite3_log() */
    public const int SQLITE_WARNING = 28;   /* Warnings from sqlite3_log() */
    public const int SQLITE_ROW = 100;  /* sqlite3_step() has another row ready */
    public const int SQLITE_DONE = 101;  /* sqlite3_step() has finished executing */
    /* end-of-error-codes */


    public const int SQLITE_VTAB_CONSTRAINT_SUPPORT = 1;
    public const int SQLITE_VTAB_INNOCUOUS = 2;
    public const int SQLITE_VTAB_DIRECTONLY = 3;
    public const int SQLITE_VTAB_USES_ALL_SCHEMAS = 4;

    /* Column numbers */
    public const int SERIES_COLUMN_VALUE = 0;
    public const int SERIES_COLUMN_START = 1;
    public const int SERIES_COLUMN_STOP = 2;
    public const int SERIES_COLUMN_STEP = 3;
    /*
    ** CAPI3REF: Fundamental Datatypes
    ** KEYWORDS: SQLITE_TEXT
    **
    ** ^(Every value in SQLite has one of five fundamental datatypes:
    **
    ** <ul>
    ** <li> 64-bit signed integer
    ** <li> 64-bit IEEE floating point number
    ** <li> string
    ** <li> BLOB
    ** <li> NULL
    ** </ul>)^
    **
    ** These constants are codes for each of those types.
    **
    ** Note that the SQLITE_TEXT constant was also used in SQLite version 2
    ** for a completely different meaning.  Software that links against both
    ** SQLite version 2 and SQLite version 3 should use SQLITE3_TEXT, not
    ** SQLITE_TEXT.
    */
    public const int SQLITE_INTEGER = 1;
    public const int SQLITE_FLOAT = 2;
    public const int SQLITE_BLOB = 4;
    public const int SQLITE_NULL = 5;
    public const int SQLITE_TEXT = 3;

    /*
    ** CAPI3REF: Virtual Table Constraint Operator Codes
    **
    ** These macros define the allowed values for the
    ** [sqlite3_index_info].aConstraint[].op field.  Each value represents
    ** an operator that is part of a constraint term in the WHERE clause of
    ** a query that uses a [virtual table].
    **
    ** ^The left-hand operand of the operator is given by the corresponding
    ** aConstraint[].iColumn field.  ^An iColumn of -1 indicates the left-hand
    ** operand is the rowid.
    ** The SQLITE_INDEX_CONSTRAINT_LIMIT and SQLITE_INDEX_CONSTRAINT_OFFSET
    ** operators have no left-hand operand, and so for those operators the
    ** corresponding aConstraint[].iColumn is meaningless and should not be
    ** used.
    **
    ** All operator values from SQLITE_INDEX_CONSTRAINT_FUNCTION through
    ** value 255 are reserved to represent functions that are overloaded
    ** by the [xFindFunction|xFindFunction method] of the virtual table
    ** implementation.
    **
    ** The right-hand operands for each constraint might be accessible using
    ** the [sqlite3_vtab_rhs_value()] interface.  Usually the right-hand
    ** operand is only available if it appears as a single constant literal
    ** in the input SQL.  If the right-hand operand is another column or an
    ** expression (even a constant expression) or a parameter, then the
    ** sqlite3_vtab_rhs_value() probably will not be able to extract it.
    ** ^The SQLITE_INDEX_CONSTRAINT_ISNULL and
    ** SQLITE_INDEX_CONSTRAINT_ISNOTNULL operators have no right-hand operand
    ** and hence calls to sqlite3_vtab_rhs_value() for those operators will
    ** always return SQLITE_NOTFOUND.
    **
    ** The collating sequence to be used for comparison can be found using
    ** the [sqlite3_vtab_collation()] interface.  For most real-world virtual
    ** tables, the collating sequence of constraints does not matter (for example
    ** because the constraints are numeric) and so the sqlite3_vtab_collation()
    ** interface is not commonly needed.
    */
    public const int SQLITE_INDEX_CONSTRAINT_EQ = 2;
    public const int SQLITE_INDEX_CONSTRAINT_GT = 4;
    public const int SQLITE_INDEX_CONSTRAINT_LE = 8;
    public const int SQLITE_INDEX_CONSTRAINT_LT = 16;
    public const int SQLITE_INDEX_CONSTRAINT_GE = 32;
    public const int SQLITE_INDEX_CONSTRAINT_MATCH = 64;
    public const int SQLITE_INDEX_CONSTRAINT_LIKE = 65;
    public const int SQLITE_INDEX_CONSTRAINT_GLOB = 66;
    public const int SQLITE_INDEX_CONSTRAINT_REGEXP = 67;
    public const int SQLITE_INDEX_CONSTRAINT_NE = 68;
    public const int SQLITE_INDEX_CONSTRAINT_ISNOT = 69;
    public const int SQLITE_INDEX_CONSTRAINT_ISNOTNULL = 70;
    public const int SQLITE_INDEX_CONSTRAINT_ISNULL = 71;
    public const int SQLITE_INDEX_CONSTRAINT_IS = 72;
    public const int SQLITE_INDEX_CONSTRAINT_LIMIT = 73;
    public const int SQLITE_INDEX_CONSTRAINT_OFFSET = 74;
    public const int SQLITE_INDEX_CONSTRAINT_FUNCTION = 150;

    public static delegate* unmanaged[Cdecl]<void*, void> SQLITE_STATIC = (delegate* unmanaged[Cdecl]<void*, void>)IntPtr.Zero;
    public static delegate* unmanaged[Cdecl]<void*, void> SQLITE_TRANSIENT = (delegate* unmanaged[Cdecl]<void*, void>)new IntPtr(-1);
}

public unsafe struct sqlite3_module
{
    public int iVersion;
    public delegate* unmanaged[Cdecl]<nint, IntPtr, int, byte**, sqlite3_vtab**, byte**, int> xCreate;
    public delegate* unmanaged[Cdecl]<nint, IntPtr, int, byte**, sqlite3_vtab**, byte**, int> xConnect;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, sqlite3_index_info*, int> xBestIndex;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, int> xDisconnect;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, int> xDestroy;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, sqlite3_vtab_cursor**, int> xOpen;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, int> xClose;
    public delegate* unmanaged[Cdecl]<
      sqlite3_vtab_cursor*,
      int, byte*,
      int, nint*, int> xFilter;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, int> xNext;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, int> xEof;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, sqlite3_context*, int, int> xColumn;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, long*, int> xRowid;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, int, nint*, long*, int> xUpdate;
    public IntPtr xBegin;
    public IntPtr xSync;
    public IntPtr xCommit;
    public IntPtr xRollback;
    public IntPtr xFindFunction;
    public IntPtr xRename;
    /* The methods above are in version 1 of the sqlite_module object. Those
    ** below are for version 2 and greater. */
    public IntPtr xSavepoint;
    public IntPtr xRelease;
    public IntPtr xRollbackTo;
    /* The methods above are in versions 1 and 2 of the sqlite_module object.
    ** Those below are for version 3 and greater.  */
    public IntPtr xShadowName;

    public delegate int VirtualModuleConnect(sqlite3 db, IntPtr pUnused, int argcUnused, IntPtr argvUnused, sqlite3_vtab** ppVtab, IntPtr pzErrUnused);
}

public unsafe struct sqlite3_vtab
{
    public readonly sqlite3_module* pModule;
    public readonly int nRef;
    public byte* zErrMsg;
}

public struct sqlite3_index_constraint
{
    public int iColumn;              /* Column constrained.  -1 for ROWID */
    public byte op;         /* Constraint operator */
    public byte usable;     /* True if this constraint is usable */
    public int iTermOffset;          /* Used internally - xBestIndex should ignore */
}

public struct sqlite3_index_orderby
{
    public int iColumn;              /* Column number */
    public byte desc;       /* True for DESC.  False for ASC. */
}

public struct sqlite3_index_constraint_usage
{
    public int argvIndex;           /* if >0, constraint is part of argv to xFilter */
    public byte omit;      /* Do not code a test for this constraint */
}

public unsafe struct sqlite3_index_info
{
    /* Inputs */
    public int nConstraint;           /* Number of entries in aConstraint */
    public sqlite3_index_constraint* aConstraint;            /* Table of WHERE clause constraints */
    public int nOrderBy;              /* Number of terms in the ORDER BY clause */
    public sqlite3_index_orderby* aOrderBy;               /* The ORDER BY clause */
    /* Outputs */
    public sqlite3_index_constraint_usage* aConstraintUsage;
    public int idxNum;                /* Number used to identify the index */
    public char* idxStr;              /* String, possibly obtained from sqlite3_malloc */
    public int needToFreeIdxStr;      /* Free idxStr using sqlite3_free() if true */
    public int orderByConsumed;       /* True if output is already ordered */
    public double estimatedCost;           /* Estimated cost of using this index */
    /* Fields below are only available in SQLite 3.8.2 and later */
    public long estimatedRows;    /* Estimated number of rows returned */
    /* Fields below are only available in SQLite 3.9.0 and later */
    public int idxFlags;              /* Mask of SQLITE_INDEX_SCAN_* flags */
    /* Fields below are only available in SQLite 3.10.0 and later */
    public ulong colUsed;    /* Input: Mask of columns used by statement */
}

public unsafe struct sqlite3_vtab_cursor
{
    public sqlite3_vtab* pVtab;
}

partial class VirtualModule
{
    const string SQLITE_DLL = "e_sqlite3";
    const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

    [LibraryImport(SQLITE_DLL, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial int sqlite3_create_module_v2(sqlite3 db, string dbName, in sqlite3_module pModule, IntPtr aux, delegate* unmanaged<void*, void> xDestroy);

    [LibraryImport(SQLITE_DLL, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial int sqlite3_drop_modules(sqlite3 db, string?[]? keepModules);

    [LibraryImport(SQLITE_DLL, StringMarshalling = StringMarshalling.Utf8)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial int sqlite3_declare_vtab(nint db, string tableDefinition);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_malloc(int size);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_malloc64(ulong size);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_realloc(void* ptr, int size);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial ulong sqlite3_msize(void* ptr);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_realloc64(void* ptr, ulong size);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void sqlite3_free(void* ptr);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_mprintf(ReadOnlySpan<byte> message);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_vsnprintf(int flags, byte* where, ReadOnlySpan<byte> message);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_vtab_config(nint db, int op);

    [LibraryImport(SQLITE_DLL, EntryPoint = nameof(sqlite3_vtab_config))]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_vtab_config2(nint db, int op, int x);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_result_int64(void* context, long x);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_result_text(void* context, byte* x, int n, delegate* unmanaged[Cdecl] <void*, void> xDel);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial long sqlite3_value_int64(nint value);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial int sqlite3_value_type(nint value);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial int sqlite3_stricmp(byte* a, byte* b);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial int sqlite3_stricmp(ReadOnlySpan<byte> a, byte* b);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial nint sqlite3_str_new(nint db);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial byte* sqlite3_errmsg(nint db);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial byte* sqlite3_str_finish(nint str);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void sqlite3_str_append(nint str, byte* toadd, int N);
}