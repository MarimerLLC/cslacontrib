	#region FilteredColumns
	// Used by CSLA Objects                                                            Guid
	public bool IsGuid(IColumn column){
		return(column.LanguageType=="timestamp");}
	public ArrayList Guid(IColumns columns){
		return FilteredColumns(columns,new Filter(IsGuid));}
	// Used in CSLA Business Objects                                                   AutoKey
	public bool AutoKey(IColumn column){
		return(column.IsAutoKey);}
	public ArrayList AutoKey(IColumns columns){
		return FilteredColumns(columns,new Filter(AutoKey));}
	public ArrayList AutoKey(IForeignKey fk){
		ArrayList l = new ArrayList();
		foreach(IColumn column in fk.ForeignTable.Columns){
			if(column.IsAutoKey && !IsIn(column,fk.ForeignColumns))l.Add(column);}
		return l;}
	// Used in CSLA Stored Procedures
	private ArrayList AutoKey(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(AutoKey));}
	// Used in CSLA Stored Procedures                                                  Updatable
	public bool Updatable(IColumn column){
		return(!NotUpdatable(column));}
	private ArrayList Updatable(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(Updatable));}
	// Used in CSLA Stored Procedures                                                  Not Updatable
	public bool NotUpdatable(IColumn column){
		return(column.IsInPrimaryKey || column.IsAutoKey || column.IsComputed);}
	private ArrayList NotUpdatable(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(NotUpdatable));}
	// Used in CSLA Stored Procedures                                                  PrimaryKey
	public bool PrimaryKey(IColumn column){
		return(column.IsInPrimaryKey);}
	private ArrayList PrimaryKey(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(PrimaryKey));}
	// Used in CSLA Stored Procedures                                                  Insertable
	public bool Insertable(IColumn column){
		return(!NotInsertable(column));}
	private ArrayList Insertable(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(Insertable));}
	// Used in CSLA Stored Procedures                                                  Not Insertable
	public bool NotInsertable(IColumn column){
		return(column.IsAutoKey || column.IsComputed);}
	private ArrayList NotInsertable(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(NotInsertable));}
	// Used in CSLA Stored Procedures & CSLA Business Objects                          Computed
	public bool Computed(IColumn column){
		return(column.IsComputed);}
	private ArrayList Computed(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(Computed));	}
	// Used in CSLA Stored Procedures                                                  Not TimeStamp
	public bool NotTimestamp(IColumn column){
		return(!Timestamp(column));}
	private ArrayList NotTimestamp(ITable tbl){
		return FilteredColumns(tbl.Columns,new Filter(NotTimestamp));}
	private ArrayList NotTimestamp(IColumns cols){
		return FilteredColumns(cols,new Filter(NotTimestamp));}
	// Used in CSLA Business Objects
	public bool IsRequired(IColumn column)
	{
		if(column.IsNullable) return false;
		if(!column.HasDefault) return true;
		// If it is the parent column it is required
		return IsParentColumn(column);
//		return false;
	}
//	public bool ForeignKey(IColumn column)
//	{
//		return(column.IsInForeignKey);
//	}
	// Used in CSLA Business Objects                                                   Automatic
	public bool IsAutomatic(IColumn column)
	{
		bool retval=column.Description.IndexOf("{auto}") >= 0;
		return retval;
	}
	// Used in CSLA Business Objects                                                   TimeStamp
	public bool Timestamp(IColumn column){
		return(column.DataTypeName=="timestamp");}
	public ArrayList IsTimestamp(IColumns columns){
		return FilteredColumns(columns,new Filter(Timestamp));}
	// Used in CSLA Stored Procedures and Business Objects
	public ArrayList FilteredColumns(IColumns columns,Filter f){
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
		{
			if(f(column))l.Add(column);
		}
		return l;
	}
	// Used in CSLA Business Objects
	public ArrayList ExcludedColumns(IColumns columns,Filter f){
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
		{
			if(!f(column))l.Add(column);
		}
		return l;
	}
	private int CountRequiredFields( IColumns Columns ){
		return Columns.Count - CountNullableFields( Columns );}
	private int CountNullableFields( IColumns Columns )
	{
		int i = 0;
		foreach( IColumn c in Columns )
		{
			if( c.IsNullable )
			{
				i++;
			}
		}
		return i;
	}
	
	private int CountUniqueFields( IColumns Columns )
	{
		int i = 0;
		foreach( IColumn c in Columns )
		{
			if( !c.IsNullable && c.IsInPrimaryKey )
			{
				i++;
			}
		}
		return i;
	}
	public ArrayList MakeList(IColumns columns)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
		if(!column.IsAutoKey && !IsAutomatic(column) && !column.IsComputed)
				l.Add(column);
		return l;
	}
	public ArrayList MakeList4(IColumns columns)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
		if((!column.IsAutoKey && !IsAutomatic(column) && !column.IsComputed ) &&
		 (column.HasDefault==false || column.ForeignKeys.Count != 0))
				l.Add(column);
		return l;
	}
	public ArrayList ReqList(IColumns columns)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
			if(IsRequired(column) && !column.IsComputed && !column.IsAutoKey)
				l.Add(column);
		return l;
	}
	public ArrayList ReqListNoDefault(IColumns columns)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
			if(IsRequired(column) && !column.IsComputed && !column.IsAutoKey && column.HasDefault)
				l.Add(column);
		return l;
	}
	public ArrayList MakeList2(IColumns columns)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
		if(!column.IsAutoKey && !column.IsComputed)
				l.Add(column);
		return l;
	}
	public ArrayList MakeList3(IColumns columns)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn column in columns)
		if(IsAutomatic(column))
				l.Add(column);
		return l;
	}
	public void BuildLists(IList cols,ref string sMakeListParamTypes, ref string sMakeListParams,
	  ref string sSetTmp, ref string sParentCheck)
	{
		string sep = "";
		string sepSet = "";
		string sPrefixType="";
		string sPrefix="";
		string sPrefixSet="";
		string sCheckSep = "if( ";
		sMakeListParamTypes = "";
		sMakeListParams = "";
		sSetTmp = "";
		sParentCheck="";
//		ArrayList parentCols = new ArrayList();
		foreach(IColumn col in cols)
		{
				sMakeListParamTypes += sep + FormatColumn("{!rtype} {!local}",col);
				sMakeListParams += sep + FormatColumn("{!local}",col);
				sSetTmp += sepSet + FormatColumn("\t\t\ttmp.{!memberprop} = {!local};",col);
				sep=", ";
				sepSet="\r\n";
				if(IsRelObj(col))  // If item is null, don't look it up
				{
					sParentCheck += sCheckSep + LocalName(RelObjProp(col)) + " != null ";
					sCheckSep = "|| ";
				}
		}
		if(sParentCheck != "")sParentCheck += ") ";
		sMakeListParamTypes = sPrefixType + sMakeListParamTypes;
		sMakeListParams = sPrefix + sMakeListParams;
		sSetTmp = sPrefixSet + sSetTmp;
		//sParentCheck=FormatColumns("{!rtype} {!local} = {autoseed};\r\n\t\t\tif (parent != null)\r\n\t\t\t{\r\n\t\t\t\t{!local} = parent;\r\n\t\t\t}\r\n\t\t\t",parentCols,"\r\n","");
		//sParentCheck="";
	}
	public bool SameList(IList lst1, IList lst2)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn col in lst1)l.Add(col.Name);
		foreach(IColumn col in lst2)
			if(l.Contains(col.Name))l.Remove(col.Name);
			else return false;
		return l.Count ==0;
	}
	public string SameList2(IList lst1, IList lst2)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn col in lst1)l.Add(col.Name);
		foreach(IColumn col in lst2)l.Remove(col.Name);
		string sList="";
		string sep="Difference: ";
		foreach(string ss in l){
			sList+=sep+ss;
			sep=", ";
		}
		return sList;
	}
	public bool SameTypes(IList lst1, IList lst2)
	{
		if(lst1.Count != lst2.Count) return false;
		for(int i=0;i<lst1.Count;i++)
		{
			IColumn col1 = (IColumn)(lst1[i]);
			IColumn col2 = (IColumn)(lst2[i]);
			if(CSLAType(col1)!=CSLAType(col2))
				return false;
		}
				return true;
	}
	public string FieldName(IColumn col)
	{
		return col.Table.Name + "." + col.Name;
	}
	public bool SameFields(IList lst1, IList lst2)
	{
		if(lst1.Count != lst2.Count) return false;
		for(int i=0;i<lst1.Count;i++)
		{
			IColumn col1 = (IColumn)(lst1[i]);
			IColumn col2 = (IColumn)(lst2[i]);
			if(FieldName(col1)!=FieldName(col2))
				return false;
		}
		return true;
	}
//	public ArrayList IsNotAutoKey(IColumns columns)
//	{
//		return ExcludedColumns(columns,new Filter(AutoKey));
//	}
//	public bool TypeString(IColumn column)
//	{
//		return(CSLAType(column)=="string");
//	}
	public bool HasDefault(IColumn column)
	{
		return column.HasDefault;
	}
	public ArrayList HasDefaults(IColumns columns)
	{
		return FilteredColumns(columns,new Filter(HasDefault));
	}
	public ArrayList HasDefaults(IForeignKey fk)
	{
		ArrayList l = new ArrayList();
		foreach(IColumn column in fk.ForeignTable.Columns)
		{
			if(column.HasDefault && !IsIn(column,fk.ForeignColumns))l.Add(column);
		}
		return l;
	}
	private string RelatedObject(IForeignKey fk, IColumn column){
		if(column.ForeignKeys.Count == 1){
			IForeignKey pk = column.ForeignKeys[0];
			string sObj = _nameSpace + "." + ClassName( pk.PrimaryTable );
			return FormatColumn(sObj + ".Get" + "({local})",column);
		}
		else
		{
			return FormatColumn("{local}",column);
		}
	}
	private string RelatedObjectType(IColumn column){
		if(column.ForeignKeys.Count == 1){
			IForeignKey pk = column.ForeignKeys[0];
			string sObj = ClassName( pk.PrimaryTable );
			return sObj + " " + LocalName(sObj);
		}
		else
		{
			return FormatColumn("{ctype} {local}",column);
		}
	}
	public string GetNewAlias2(IForeignKey fk,IForeignKey pk)
	{
		// First determine if there are more than one key linking two tables
		int iCount = 0;
		foreach(IForeignKey tk in fk.ForeignTable.ForeignKeys)
		{
//				WriteLine("fk {0} pk {1} pk.PrimaryTable.Name {2} pk.ForeignTable.Name {3} fk.ForeignTable.Name {4}",
//					fk.Name,pk.Name,pk.PrimaryTable.Name,pk.ForeignTable.Name,fk.ForeignTable.Name);
		
			if(tk.PrimaryTable == pk.PrimaryTable)iCount++;
		}
		if(iCount==0)return "";
		return pk.ForeignColumns[0].Name;
	}
	public string GetNewAlias(IForeignKey fk,IForeignKey pk)
	{
		// First determine if there are more than one key linking two tables
		int iCount = 0;
//		WriteLine("fk {0} fk.Primary {1} fk.Foreign {2}",fk.Name,fk.PrimaryTable.Name,fk.ForeignTable.Name);
//		WriteLine("  pk {0} pk.Primary {1} pk.Foreign {2}",pk.Name,pk.PrimaryTable.Name,pk.ForeignTable.Name);
		foreach(IForeignKey tk in fk.ForeignTable.ForeignKeys)
		{
			if(tk.ForeignTable == pk.ForeignTable && tk.PrimaryTable == pk.PrimaryTable){
//				WriteLine("    tk {0} tk.Primary {1} tk.Foreign {2}",tk.Name,tk.PrimaryTable.Name,tk.ForeignTable.Name);
				iCount++;
			}
		}
		if(iCount==1)return "";
//		WriteLine("iCount {0}",iCount);
		return pk.ForeignColumns[0].Name;
	}
	private string RelatedObjectType2(IForeignKey fk,IColumn column){
		if(column.ForeignKeys.Count == 1){
			IForeignKey pk = column.ForeignKeys[0];
			string sObj = ClassName( pk.PrimaryTable );
			//return sObj + " " + LocalName(sObj) + column.Name;
			return sObj + " " + LocalName(sObj) + GetNewAlias(fk,pk);
		}
		else
		{
			return FormatColumn("{ctype} {local}",column);
		}
	}
	private bool IsPrimaryKey(IColumn col)
	{
		return (col.IsInPrimaryKey && col.Table.PrimaryKeys.Count == 1);
	}
	private IForeignKey RelObjFK(IColumn col)
	{
		if(!col.IsComputed && !col.IsAutoKey && !IsPrimaryKey(col) && col.ForeignKeys.Count==1)
		{
			IForeignKey fk = col.ForeignKeys[0];
//			if(fk.PrimaryTable.Name != fk.ForeignTable.Name)
//			{
//			return (col.Name=="RangeID"?fk:null);
				return fk;
//			}
		}
		return null;
	}
	private bool IsRelObj(IColumn col)
	{
		if(col.Table.Name != _workingTable.Name && col.IsInPrimaryKey && col.Table.PrimaryKeys.Count ==1)
			return true;
		IForeignKey fk = RelObjFK(col);
		if(fk != null)
			return true;
		else
			return false;
	}
	private string RelObjType(IColumn col)
	{
		if(col.Table.Name != _workingTable.Name && col.IsInPrimaryKey && col.Table.PrimaryKeys.Count ==1)
			return ClassName( col.Table );
		IForeignKey fk = RelObjFK(col);
		if(fk != null)
			return ClassName( fk.PrimaryTable );
		else
			return CSLAType(col);
	}
	private string RelObjProp(IColumn col)
	{
		if(col.Table.Name != _workingTable.Name && col.IsInPrimaryKey && col.Table.PrimaryKeys.Count ==1)
				return "My" + ClassName( col.Table );
		IForeignKey fk = RelObjFK(col);
		if(fk != null)
			if(fk.PrimaryTable.Name == fk.ForeignTable.Name)
				return "My" + ParentName(fk) + GetNewAlias(fk,fk);
			else
				return "My" + ClassName( fk.PrimaryTable ) + GetNewAlias(fk,fk);
		else
			return PropertyName(col);
	}
	private string RelObjCol(IColumn col)
	{
		IForeignKey fk = RelObjFK(col);
		if(fk != null)
			return PropertyName(fk.PrimaryColumns[0]);
		else
			return PropertyName(col);
	}
	private string RelObjEmpty(IColumn col)
	{
		IForeignKey fk = RelObjFK(col);
		if(col.IsNullable)return "null";
		if(fk != null && fk.PrimaryTable.Name == fk.ForeignTable.Name)
			return MemberName(fk.PrimaryColumns[0]);
		if(CSLAType(col)=="string")return "null";
		return "0";
	}
	private string RelObjTypeCast(IColumn col)
	{
		IForeignKey fk = RelObjFK(col);
		if(col.IsNullable)return "(" + CSLAType(col,"") + ")";
		return "";
	}
	private ArrayList _TableStack = new ArrayList();
	private void PushTable(ITable tbl)
	{
		_TableStack.Add(_workingTable);
		_workingTable=tbl;
	}
	private void PopTable()
	{
		if(_TableStack.Count > 0)
		{
			_workingTable=(ITable) _TableStack[_TableStack.Count-1];
			_TableStack.RemoveAt(_TableStack.Count-1);
		}
	}
	private bool ForeignRequired(IForeignKey pk)
	{
		bool bRequired=true;
		foreach(IColumn col in pk.ForeignColumns)
			bRequired &= !col.IsNullable;
		return bRequired;
	}
	private bool ForeignPrimary(IForeignKey fk)
	{
		bool bPrimary=true;
		foreach(IColumn col in fk.ForeignColumns)
		{
			bPrimary &= col.IsInPrimaryKey;
		}
		return bPrimary;
	}
	public int FindColumn(IList cols, IColumn col)
	{
		for(int i=0;i<cols.Count;i++)
			if(((IColumn) cols[i]).Name==col.Name)
				return i;
		return -1;
	}
	public ArrayList FindUniqueChildren(IForeignKey fk){
		ArrayList retval = new ArrayList();
		foreach(IIndex ind in fk.ForeignTable.Indexes)
		{
			if(ind.Unique)
			{
				ArrayList uniqueColumns = new ArrayList();
				foreach(IColumn col in ind.Columns)
				{
					uniqueColumns.Add(col);
				}
				foreach(IColumn col in fk.ForeignColumns)
				{
					int ii = FindColumn(uniqueColumns,col);
					if(ii >= 0)
					{
						uniqueColumns.RemoveAt(ii);
					}
					else
					{
						uniqueColumns.Clear();
					}
				}
				if(uniqueColumns.Count > 0)// This is a uniqueIndex which includes the parent columns
				{
					//ShowColumns(uniqueColumns,4,"Unique Index " + ind.Name);
					retval.Add(uniqueColumns);
				}
			}
		}
		return retval;
	}
	public bool ContainsList(IList lst1, IList lst2)
	{
		foreach(IColumn col in lst2)
			if(FindColumn(lst1,col)<0)return false;
		return true;
	}
	public IList FindUnique(ArrayList lst, IList cols)
	{
		foreach(IList lstcols in lst)
		{
			if(ContainsList(cols,lstcols))return lstcols;
		}
		return null;
	}
	//public ArrayList zzFilteredColumnsAny(IColumns columns,Filter [] fs){
	//	ArrayList l = new ArrayList();
	//	foreach(IColumn column in columns)
	//	{
	//		bool check = false;
	//		foreach(Filter f in fs)check |= f(column);
	//		if(check)l.Add(column);
	//	}
	//	return l;
	//}
	//public ArrayList zzExcludedColumnsAny(IColumns columns,Filter [] fs){
	//	ArrayList l = new ArrayList();
	//	foreach(IColumn column in columns)
	//	{
	//		bool check = false;
	//		foreach(Filter f in fs)check |= f(column);
	//		if(!check)l.Add(column);
	//	}
	//	return l;
	//}
	#endregion
