using SQLitePCL;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SqliteVirtualTables;

public unsafe struct sqlite3_module
{
    public int iVersion;
    public IntPtr xCreate;
    public delegate* unmanaged[Cdecl]<nint, IntPtr, int, IntPtr, sqlite3_vtab**, IntPtr, int> xConnect;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, sqlite3_index_info*, int> xBestIndex;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, int> xDisconnect;
    public IntPtr xDestroy;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab*, sqlite3_vtab_cursor**, int> xOpen;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, int> xClose;
    public delegate* unmanaged[Cdecl]<
      sqlite3_vtab_cursor*,
      int, IntPtr,
      int, nint*, int> xFilter;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, int> xNext;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, int> xEof;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, sqlite3_context*, int, int> xColumn;
    public delegate* unmanaged[Cdecl]<sqlite3_vtab_cursor*, long*, int> xRowid;
    public IntPtr xUpdate;
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
    public static unsafe partial void sqlite3_free(void* ptr);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial void* sqlite3_mprintf(ReadOnlySpan<byte> message);

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
    public static unsafe partial long sqlite3_value_int64(nint value);

    [LibraryImport(SQLITE_DLL)]
    [UnmanagedCallConv(CallConvs = new Type[] { typeof(CallConvCdecl) })]
    public static unsafe partial int sqlite3_value_type(nint value);
}