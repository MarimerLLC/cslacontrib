// Foreign Key Processing
	public delegate void ProcessFK(IForeignKey fk,string alias);
	public string ParentName(IForeignKey fk)
	{
		if(fk == null)return "Parent";
		string sKey = fk.ForeignColumns[0].Name;
		if(sKey.StartsWith("Prev") || sKey.StartsWith("Prv"))return "Previous";
		if(sKey.StartsWith("Next") || sKey.StartsWith("Nxt"))return "Next";
		return "Parent";
	}
	public string ChildName(IForeignKey fk)
	{
		string sKey = fk.ForeignColumns[0].Name;
		if(sKey.StartsWith("Prev") || sKey.StartsWith("Prv"))return "Next";
		if(sKey.StartsWith("Next") || sKey.StartsWith("Nxt"))return "Previous";
		return "Child";
	}
	public string ChildrenName(IForeignKey fk)
	{
		string sKey = fk.ForeignColumns[0].Name;
		if(sKey.StartsWith("Prev") || sKey.StartsWith("Prv"))return "Next";
		if(sKey.StartsWith("Next") || sKey.StartsWith("Nxt"))return "Previous";
		return "Children";
	}
	public string GetAlias(Hashtable hTbl,ITable tbl)
	{
		string sAlias = tbl.Alias;
		if(!hTbl.Contains(sAlias))
		{
			hTbl[sAlias]=0;
			return "";
		}
//		WriteLine("-- Hashtable count = {0}, TableName = [{1}]",hTbl.Count,tbl.Name);
		int iAlias = 1+ (int)hTbl[sAlias];
		hTbl[sAlias]=iAlias;
//		return "_" + iAlias.ToString();
		return "_" + "ABCDEFGHIJKLMNOP".Substring(iAlias,1);
	}
	public string GetAlias(IForeignKey fk)
	{
		if(fk.PrimaryTable.Name == fk.ForeignTable.Name)return "";
		// First I need to check to see if there are more than one Foreign Keys pointing to the same Primary Table
		int count=0;
		foreach(IForeignKey fk1 in fk.ForeignTable.ForeignKeys)
		{
			if(fk1.PrimaryTable.Name == fk.PrimaryTable.Name) count++;
		}
		if(count > 1)
			return FormatColumns("_{prop}",fk.ForeignColumns,"","");
		return "";
	}
	public void ProcessFKAlias(ITable tbl,ProcessFK pfk)
	{
		Hashtable dicByNames = new Hashtable();
		foreach(ForeignKey fk in tbl.ForeignKeys)
		{
			if(fk.ForeignTable == tbl)
			{
				pfk(fk,GetAlias(dicByNames,fk.PrimaryTable));
			}
		}	
	}
	private bool IsPrimaryKey(IForeignKey fk)
	{
//		bool retval=true;
//		foreach(IColumn c in fk.ForeignColumns)retval &= c.IsInPrimaryKey;
		return SameList(fk.ForeignTable.PrimaryKeys,fk.ForeignColumns);
//		return retval;
//	return IsPrimaryKey(fk.ForeignColumns);
	}
	private bool IsPrimaryKey1(IForeignKey fk)
	{
//		bool retval=true;
//		foreach(IColumn c in fk.ForeignColumns)retval &= c.IsInPrimaryKey;
		return SameList(fk.PrimaryTable.PrimaryKeys,fk.PrimaryColumns);
//		return retval;
//	return IsPrimaryKey(fk.ForeignColumns);
	}
	private string ColumnList(IColumns cols)
	{
		string retval="";
		string sep = "";
		foreach(IColumn col in cols)
		{
			retval += sep + col.Name;
			sep = ", ";
		}
		return retval;
	}
	private bool IsPrimaryKey(IColumns cols)
	{
		ITable tbl = cols[0].Table;
		int match = tbl.PrimaryKeys.Count;
		foreach(IColumn col in cols)
		{
			if(col.Table.Name != tbl.Name)
			{
//				WriteLine("Table Mismatch - {0},{1}",col.Table.Name,tbl.Name);
				return false; // If it points to multiple tables, it is not a primary key
			}
			if(col.IsInPrimaryKey)match --;
			else
			{
//				WriteLine("Not In Primary Key {0} in {1} ({2})",col.Name, tbl.Name, ColumnList(tbl.PrimaryKeys));
				return false;
			}
		}
//		WriteLine("Match = {0}",match);
		return match==0;
	}
	private bool IsPrimaryKey(IColumns cols1,IColumns cols2)
	{
		ITable tbl = cols1[0].Table;
		int match = tbl.PrimaryKeys.Count;
		foreach(IColumn col in cols1)
		{
			if(col.Table.Name != tbl.Name)
			{
//				WriteLine("Table Mismatch - {0},{1}",col.Table.Name,tbl.Name);
				return false; // If it points to multiple tables, it is not a primary key
			}
			if(col.IsInPrimaryKey)match --;
			else
			{
//				WriteLine("Not In Primary Key {0} in {1} ({2})",col.Name, tbl.Name, ColumnList(tbl.PrimaryKeys));
				return false;
			}
		}
//		WriteLine("Match = {0}",match);
		foreach(IColumn col in cols2)
		{
			if(col.Table.Name != tbl.Name)
			{
//				WriteLine("Table Mismatch - {0},{1}",col.Table.Name,tbl.Name);
				return false; // If it points to multiple tables, it is not a primary key
			}
			if(col.IsInPrimaryKey)match --;
			else
			{
//				WriteLine("Not In Primary Key {0} in {1} ({2})",col.Name, tbl.Name, ColumnList(tbl.PrimaryKeys));
				return false;
			}
		}
//		WriteLine("Match = {0}",match);
		return match==0;
	}
	private bool ManyToMany(ITable tbl,IForeignKey fk,IForeignKey pk)
	{
		if(tbl != fk.PrimaryTable)return false;// It should be the Primary Table
		if(tbl == pk.PrimaryTable)return false;// It should not loop back to the Primary Table
		if(fk.PrimaryTable == fk.ForeignTable)return false;// Ignore Parent/Child relationships
		if(pk.PrimaryTable == pk.ForeignTable)return false;// Ignore Parent/Child relationships
		// Now for the detail check -  the primary columns in each foreign key should be the primary key in the primary table
//		WriteLine("Checking Primary Keys - {0}",tbl.Name);
		if(!IsPrimaryKey(fk.PrimaryColumns))return false;
		if(!IsPrimaryKey(pk.PrimaryColumns))return false;
		// And the foreign columns from both foreign keys should be the primary key in the foreigntable
//		WriteLine("Checking Foreign Keys - {0}",tbl.Name);
		if(!IsPrimaryKey(fk.ForeignColumns,pk.ForeignColumns))return false;
		return true;
	}
	private bool OneToOne(ITable tbl)
	{
		// Check to see if the Primary Key is also the Foreign Part of a Foreign Key
		foreach(IColumn col in tbl.PrimaryKeys)
		{
			if(IsPrimaryKey(col))
			{
				foreach(IForeignKey fk in col.ForeignKeys)
				{
	//				WriteLine("{0} {1} {2} {3} {4}",fk.Name,fk.PrimaryTable.Name,fk.ForeignTable.Name,
	//					fk.ForeignTable.Name == tbl.Name, fk.PrimaryTable.Name != tbl.Name);
					if(fk.ForeignTable.Name == tbl.Name && fk.PrimaryTable.Name != tbl.Name )
						return true;
				}
			}
		}
		return false;
	}
