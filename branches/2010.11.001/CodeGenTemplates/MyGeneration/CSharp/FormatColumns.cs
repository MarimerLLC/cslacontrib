//// FormatColumns
//	private string FormatColumnOld(string sFormat,IColumn column,string sAlias)
//	{
//		string s=sFormat;
//		s=s.Replace("{member}",ColumnToMemberVariable(column));//m - Member _firstname
//		s=s.Replace("{fmember}",(column.Table.Name==_workingTable.Name?"":"_" + ToClassName(column.Table.Name).ToLower() + sAlias)+ColumnToMemberVariable(column));//m - Member _firstname
//		s=s.Replace("{pmember}",(column.Table.Name==_workingTable.Name?"":ToClassName(column.Table.Name).ToLower() + sAlias)+"."+ColumnToPropertyName(column));//m - Member _firstname
//		s=s.Replace("{local}",ColumnToMemberVariable(column).Substring(1));//l Local firstname
//		s=s.Replace("{prop}",ColumnToPropertyName(column));
//		s=s.Replace("{memberprop}",(column.IsInPrimaryKey ? ColumnToMemberVariable(column) : ColumnToPropertyName(column)));
//		s=s.Replace("{@}",ColumnToParameterName(column));
//		s=s.Replace("{@new}",ColumnToNewParameterName(column));
//		string cType = ColumnToCSLAType(column);
//		s=s.Replace("{ctype}",cType);
//		s=s.Replace("{rmember}",vlnReturnMember(cType));
//		s=s.Replace("{rtype}",vlnReturnType(cType));
////		s=s.Replace("{dbtype}",ColumnToMemberVariable(column).Substring(1));
//		s=s.Replace("{name}",column.Name);
//		s=s.Replace("{class}",ToClassName(column.Table.Alias.Replace( " ", "" )));
//		s=s.Replace("{fname}",(column.Table.Name==_workingTable.Name?"":column.Table.Name.Replace(" ","")+sAlias+"_")+column.Name);
//		s=s.Replace("{sqltype}",ColumnToSQLDbType(column));
//		s=s.Replace("{?ref}",(column.IsComputed || column.IsAutoKey ? "ref " : ""));
//		s=s.Replace("{?dbtype}",(ColumnToCSLAType( column )=="SmartDate" ? ".DBValue" : ""));
//		s=s.Replace("{?dbprefix}",(ColumnToCSLAType( column )=="SmartDate" ? "new SmartDate(" : ""));
//		s=s.Replace("{?dbsuffix}",(ColumnToCSLAType( column )=="SmartDate" ? ").DBValue" : ""));
//		s=s.Replace("{?dbsuff}",(ColumnToCSLAType( column )=="SmartDate" ? ")" : ""));
//		s=s.Replace("{default}",ColumnDefault(column));		
//		//s=s.Replace("{xxx}","");
//		return s;
//	}
//	private string FormatColumnOld(string sFormat,IColumn column)
//	{
//		return FormatColumn(sFormat,column,"");
//	}
//	private string ColumnToSQLDbType(IColumn column)
//	{
//		switch(column.DataTypeName)
//		{
//			case "timestamp":
//				return "SqlDbType.Timestamp";
//			case "int":
//				return "SqlDbType.Int";
//			default:
//				//return "//TODO: Need to fix ColumnToSQLDbType" + " " + column.DataType + " " + column.DataTypeName + " " + column.DataTypeNameComplete + " " + column.DbTargetType;
//				return "SqlDBType./* " + column.DataTypeNameComplete + " " + column.DbTargetType + "*/";
//		}
//	}
//	private string vlnReturnType(string sType)
//	{
//		string sReturnType=sType;
//		if(sType=="SmartDate")sReturnType="string";
//		return sReturnType;		
//	}
//	private string vlnReturnMember(string sType)
//	{
//		string sReturnMember="";
//		return sReturnMember;		
//	}
//	private string FormatColumns(string sFormat,IColumns columns)
//	{
//		return FormatColumns(sFormat,columns,"");
//	}
//	private string FormatColumns(string sFormat,IList columns,string sep)
//	{
//		string s="";
//		string ssep="";
//		foreach(IColumn column in columns)
//		{
//			s+=ssep+FormatColumn(sFormat,column);
//			ssep=sep;
//		}
//		return s;
//	}
//	private string ColumnToMemberVariable( IColumn Column )
//	{
//		return _prefix + UniqueColumn( Column ).ToLower();
//	}
//	
//	private string ColumnToPropertyName( IColumn Column )
//	{
//		return ToPascalCase( UniqueColumn( Column ) );
//	}
//	
//	private string ColumnFKToClassName( IColumn c )
//	{
//		return ToPascalCase( c.ForeignKeys[0].PrimaryTable.Alias.Replace( " ", "" ) );
//	}
//	private string ColumnToParameterName(IColumn col)
//	{
//		return "@" + ToLeadingLower(col.Name).Replace(" ","");
//	}
//	private string ColumnToNewParameterName(IColumn col)
//	{
//		return "@new" + col.Name.Replace(" ","");
//	}
//	private string ColumnToArgumentName( IColumn Column )
//	{
//		return UniqueColumn( Column ).ToLower();
//	}
//	
//	private string ColumnToNHibernateProperty( IColumn Column )
//	{
//		return _prefix + UniqueColumn( Column );
//	}
//	
//	private string UniqueColumn( IColumn Column )
//	{
//		string c = Column.Alias.Replace( " ", "" );
//		if( Column.Table != null && Column.Table.Alias.Replace( " ", "" ) == c )
//		{
//			c += "Name";
//		}
//		if( Column.View != null && Column.View.Alias.Replace( " ", "" ) == c )
//		{
//			c += "Name";
//		}
//		return c;
//	}
//	
//	// nhibernate doesn't have these, so use the existing types
//	private string ColumnToCSLAType( IColumn Column )
//	{
//		string retVal = Column.LanguageType;
//		
//		switch( Column.LanguageType )
//		{
//			case "DateTime":
//				if(Column.Description.IndexOf("{datetime}")>=0)
//					retVal="DateTime";
//				else
//					retVal = "SmartDate";
//				break;
//	//		case "uint":
//	//			retVal = "int";
//	//			break;
//	//		case "ulong":
//	//			retVal = "long";
//	//			break;
//	//		case "ushort":
//	//			retVal = "short";
//	//			break;
//		}
//		return retVal;
//	}
//	private string ColumnToDefault( IColumn Column )
//	{
//		string retVal=";";
//		if(Column.DataTypeName=="timestamp")
//		{
//			retVal = " = new byte[8];//timestamp";
//		}
//		else
//		{
//			//if(Column.Default != ""){
//			//	retVal = ConvertDefault(Column) + ";// TODO: Default from DB " + RemoveParens(Column.Default) + " ";
//			//}
//			//else
//			//{
//				switch( ColumnToCSLAType(Column ) )
//				{
//					case "string":
//						retVal = " = string.Empty;";
//						break;
//					case "DateTime":
//						retVal = " = new DateTime();";
//						break;
//					case "SmartDate":
//						retVal = " = string.Empty;";
//						break;
//					//case "Guid":
//					//	retVal = "=new Guid();";
//					//	break;
//					default:
//						// nothing to do here
//						break;
//				}
//			//}
//		}
//		return retVal;
//	}
//	string ConvertDefault(IColumn column)
//	{
//		string s = RemoveParens(column.Default);
//		switch(s)
//		{
//			case "getdate()":
//				if(ColumnToCSLAType(column)=="DateTime")
//					s="=DateTime.Now";
//				else
//					s="=DateTime.Now.ToShortDateString()";
//				break;
//			case "upper(suser_sname())":
//				s= "=Environment.UserName.ToUpper()";
//				break;
//			case "suser_sname()":
//				s="=Environment.UserName";
//				break;
//			default:
//				if(IsNumeric(s))s="=" + s;
//				else s="";
//				break;
//		}
//		return s;
//	}
//	string ColumnDefault(IColumn column)
//	{
//		string s = RemoveParens(column.Default);
//		switch(s)
//		{
//			case "getdate()":
//				if(ColumnToCSLAType(column)=="DateTime")
//					s="DateTime.Now";
//				else
//					s="DateTime.Now.ToShortDateString()";
//				break;
//			case "upper(suser_sname())":
//				s= "Environment.UserName.ToUpper()";
//				break;
//			case "suser_sname()":
//				s="Environment.UserName";
//				break;
//			default:
//				if(IsNumeric(s))s="" + s;
//				else s="";
//				break;
//		}
//		return s;
//	}
//	private string ToLeadingCaps( string name )
//	{
//		char[] chars = name.ToLower().ToCharArray();
//		chars[0] = Char.ToUpper( chars[0] );
//		return new string( chars );
//	}
//	
//	
//	private string ToPascalCase( string name )
//	{
//		string notStartingAlpha = Regex.Replace( name, "^[^a-zA-Z]+", "" );
//		string workingString = ToLowerExceptCamelCase( notStartingAlpha );
//		workingString = RemoveSeparatorAndCapNext( workingString );
//		return workingString;
//	}
//	private string ToClassName(string name)
//	{
//		return Regex.Replace(ToPascalCase(name),"s$","");
//	}
//	private string RemoveSeparatorAndCapNext( string input )
//	{
//		string dashUnderscore = "-_";
//		string workingString = input;
//		char[] chars = workingString.ToCharArray();
//		int under = workingString.IndexOfAny( dashUnderscore.ToCharArray() );
//		while( under > -1 )
//		{
//			chars[ under + 1 ] = Char.ToUpper( chars[ under + 1 ], CultureInfo.InvariantCulture );
//			workingString = new String( chars );
//			under = workingString.IndexOfAny( dashUnderscore.ToCharArray(), under + 1 );
//		}
//		chars[ 0 ] = Char.ToUpper( chars[ 0 ], CultureInfo.InvariantCulture );
//		workingString = new string( chars );
//		return Regex.Replace( workingString, "[-_]", "" );
//	}
//	private string ToLowerExceptCamelCase( string input )
//	{
//		char[] chars = input.ToCharArray();
//		for( int i = 0; i < chars.Length; i++ )
//		{
//			int left = ( i > 0 ? i - 1 : i );
//			int right = ( i < chars.Length - 1 ? i + 1 : i );
//			if( i != left && i != right )
//			{
//				if( Char.IsUpper( chars[i] ) && Char.IsLetter( chars[ left ] ) && Char.IsUpper( chars[ left ] ) )
//				{
//					chars[i] = Char.ToLower( chars[i], CultureInfo.InvariantCulture );
//				}
//				else if( Char.IsUpper( chars[i] ) && Char.IsLetter( chars[ right ] ) && Char.IsUpper( chars[ right ] ) )
//				{
//					chars[i] = Char.ToLower( chars[i], CultureInfo.InvariantCulture );
//				}
//				else if( Char.IsUpper( chars[i] ) && !Char.IsLetter( chars[ right ] ) )
//				{
//					chars[i] = Char.ToLower( chars[i], CultureInfo.InvariantCulture );
//				}
//			}
//		}
//		chars[ chars.Length - 1 ] = Char.ToLower( chars[ chars.Length - 1 ], CultureInfo.InvariantCulture );
//		return new string( chars );
//	}
	private string FormatColumnNew(string sFormat,IColumn column)
	{
		return FormatColumnNew(sFormat,column,"");
	}
	private string FormatColumnNew(string sFormat,IColumn col,string sAlias)
	{
		string s=sFormat;
		IColumn column = col.Table.Columns[col.Name];
//		IForeignKey fkkk = RelObjFK(column);
		string relProp=RelObjProp(column);
		string relType=RelObjType(column);
		string suffix = "";//(PropertyName(column)==ClassName(column.Table)?"Fld":"");
		s=s.Replace("{member}",MemberName(column)+suffix);//m - Member _firstname
		s=s.Replace("{fmember}",(column.Table.Alias==_tableName?"":_prefix + ClassName(column.Table)+sAlias)+MemberName(column)+suffix);//m - Member _firstname
		s=s.Replace("{pmember}",(column.Table.Alias==_tableName?_prefix:LocalName(ClassName(column.Table))+".")+PropertyName(column)+suffix);//m - Member _firstname
		s=s.Replace("{rmember}",LocalName(ClassName(column.Table))+"."+PropertyName(column));//m - Member _firstname
		s=s.Replace("{!prop}",relProp);
		s=s.Replace("{!member}",MemberName(relProp));
		s=s.Replace("{!membercolumn}",MemberName(relProp) + (IsRelObj(column)?"." + RelObjCol(column):""));
		s=s.Replace("{!local}",LocalName(relProp));
		s=s.Replace("{!localcolumn}",LocalName(relProp) + (IsRelObj(column)?"." + RelObjCol(column):""));
		s=s.Replace("{!type}",relType);
		s=s.Replace("{!rtype}",ReturnType(relType));
		s=s.Replace("{!memberprop}",(column.IsInPrimaryKey ? MemberName(relProp) : relProp)+suffix);
		s=s.Replace("{!propmember}",(IsRelObj(column) ? relProp : MemberName(relProp))+suffix);
		s=s.Replace("{!column}",RelObjCol(column));
		s=s.Replace("{!empty}",RelObjEmpty(column));
		s=s.Replace("{!typecast}",RelObjTypeCast(column));
		s=s.Replace("{local}",LocalName(column)+suffix);//l Local firstname
		s=s.Replace("{prop}",PropertyName(column)+suffix);
		s=s.Replace("{memberprop}",(column.IsInPrimaryKey ? MemberName(column) : PropertyName(column))+suffix);
		s=s.Replace("{@}",ParameterName(column));
		s=s.Replace("{@new}",NewParameterName(column));
		string cType = CSLAType(column);
		s=s.Replace("{ctype}",cType);
		s=s.Replace("{rmember}",ReturnMember(cType));
		s=s.Replace("{rtype}",ReturnType(cType));
//		s=s.Replace("{dbtype}",LocalName(column));
		s=s.Replace("{dtype}",DBType(column));
		s=s.Replace("{name}",column.Name);
		s=s.Replace("{class}",ClassName(column.Table));
		s=s.Replace("{fname}",(column.Table.Alias==_tableName?"":ClassName(column.Table)+sAlias+"_")+column.Name);
		s=s.Replace("{sqltype}",column.DbTargetType);
		s=s.Replace("{?ref}",(column.IsComputed || column.IsAutoKey ? "ref " : ""));
		s=s.Replace("{?dbtype}",(CSLAType( column )=="SmartDate" ? ".DBValue" : ""));
		s=s.Replace("{?dbprefix}",(CSLAType( column )=="SmartDate" ? "new SmartDate(" : ""));
		s=s.Replace("{?dbsuffix}",(CSLAType( column )=="SmartDate" ? ").DBValue" : ""));
		s=s.Replace("{?dbsuff}",(CSLAType( column )=="SmartDate" ? ")" : ""));
		s=s.Replace("{default}",DefaultValue(column));		
		s=s.Replace("{?output}",(column.IsComputed || column.IsAutoKey ? " output" : ""));
		s=s.Replace("{@key}",ParamKeyName(column));
		s=s.Replace("{tbl}",column.Table.Name + sAlias);
		s=s.Replace("{?null}",(column.IsNullable?"=null":""));
		s=s.Replace("{parent}",parentName(column));
		s=s.Replace("{Parent}",ParentName(column));
		s=s.Replace("{ParentType}",ParentTypeName(column));
		s=s.Replace("{autoseed}",AutoSeed(column));
		if(s.Contains("{ifLogic"))
		{
			string ifLogic="";
			switch (CSLAType(column))
			{
				case("DateTime"):
					ifLogic = "if ({name}.Year >= 1753 && {name}.Year <= 9999) ";
					break;
				case("DateTime?"):
					ifLogic = "if ({name} != null && ((DateTime){name}).Year >= 1753 && ((DateTime){name}).Year <= 9999) ";
					break;
				default:
					if(column.IsNullable && IsRelObj(column) && s.Contains("{ifLogicL"))
						ifLogic="if({name} != null)";
					break;
			}
			s=s.Replace("{ifLogicL}",ifLogic.Replace("{name}",FormatColumnNew("{!local}{?dbtype}",column,sAlias)));
			s=s.Replace("{ifLogicP}",ifLogic.Replace("{name}",FormatColumnNew("{?dbprefix}{member}{?dbsuffix}",column,sAlias)));
			s=s.Replace("{ifLogic@}",ifLogic.Replace("{name}",FormatColumnNew("criteria.{prop}{?dbtype}",column,sAlias)));
			//string name = FormatColumnNew("{local}{?dbtype}",column,sAlias);
			//s=s.Replace("{ifLogicL}",(CSLAType(column)=="DateTime"?"if (" + name + ".Year >= 1753 && " + name + ".Year <= 9999) ":"" ));
			//name = FormatColumnNew("{?dbprefix}{member}{?dbsuffix}",column,sAlias);
			//s=s.Replace("{ifLogicP}",(CSLAType(column)=="DateTime"?"if (" + name + ".Year >= 1753 && " + name + ".Year <= 9999) ":"" ));
			//name = FormatColumnNew("criteria.{prop}{?dbtype}",column,sAlias);
			//s=s.Replace("{ifLogic@}",(CSLAType(column)=="DateTime"?"if (" + name + ".Year >= 1753 && " + name + ".Year <= 9999) ":"" ));
		}
		return s;
	}
	private string FormatColumnFKNew(string sFormat,IColumn column)
	{
		string s=sFormat;
		s=s.Replace("{fkname}",column.Name);
		s=s.Replace("{fktable}",column.Table.Name);
		return s;
	}
	private string FormatPKColumns(string sFormat,IForeignKey pk,string sep,string sPrefix,ref string ssep,string sAlias)
	{
		string s="";
		foreach(IColumn column in pk.PrimaryTable.Columns)
		{
			if(!IsIn(column,pk.PrimaryColumns) && column.DataTypeName != "timestamp")
			{
				s+=ssep+sPrefix+FormatColumn(sFormat,column,sAlias);
				ssep=sep;
			}
		}
		return s;
	}
	private string FormatFKColumns(string sFormat,IForeignKey fk,string sep,string sPrefix,ref string ssep)
	{
		string s="";
		foreach(IColumn column in fk.ForeignTable.Columns)
		{
			//if(!IsIn(column,fk.ForeignColumns))
			//{
				s+=ssep+sPrefix+FormatColumn(sFormat,column);
				ssep=sep;
			//}
		}
		return s;
	}
	private bool IsIn(IColumn column,IColumns columns)
	{
		foreach(IColumn col in columns)
			if(col.Name==column.Name && col.Table.Name == column.Table.Name)
				return true;
		return false;
	}
	private string FormatColumns(string sFormat,IList columns,ref string sep,string sPrefix,ref string ssep)
	{
		string s="";
		foreach(IColumn column in columns)
		{
			s+=ssep+sPrefix+FormatColumn(sFormat,column);
			ssep=sep;
		}
		return s;
	}
	private string FormatColumns(string sFormat,IList columns,string sep)
	{
		return FormatColumns(sFormat,columns,sep,"");
	}
	private string FormatColumns(string sFormat,IList columns,string sep,string sPrefix)
	{
		string s="";
		string ssep="";
		foreach(IColumn column in columns)
		{
			s+=ssep+sPrefix+FormatColumn(sFormat,column);
			ssep=sep;
		}
		return s;
	}
	private string FormatColumn(string sFormat,IColumn column)
	{
		return FormatColumn(sFormat,column,"");
	}
	private string FormatColumn(string sFormat,IColumn column,string sAlias)
	{
		return FormatColumnNew(sFormat,column,sAlias);
	}
	private string FormatColumns(string sFormat,IForeignKey FK,string sep,string sPrefix)
	{
		string s="";
		string ssep="";
		for(int i = 0;i<FK.PrimaryColumns.Count;i++)
		{
			s+=ssep+sPrefix+FormatColumnFK(FormatColumn(sFormat,FK.PrimaryColumns[i]),FK.ForeignColumns[i]);
			ssep=sep;
		}
		return s;
	}
	private string FormatColumnFK(string sFormat,IColumn fcolumn)
	{
		return FormatColumnFKNew(sFormat,fcolumn);
	}
	private string FormatColumns2(string sFormat,IForeignKey FK,string sep,string sPrefix)
	{
		string s="";
		string ssep="";
		for(int i = 0;i<FK.PrimaryColumns.Count;i++)
		{
			s+=ssep+sPrefix+FormatColumnFK(FormatColumn(sFormat,FK.ForeignColumns[i]),FK.ForeignColumns[i]);
			ssep=sep;
		}
		return s;
	}
	private string FormatColumns(Hashtable dicFormat,IList columns,string sep,string sPrefix)
	{
		string s="";
		string ssep="";
		foreach(IColumn column in columns)
		{
			string sFmt = GetFormat(dicFormat,column);
			if(sFmt != null)
			{
				s+=ssep+sPrefix+FormatColumn(sFmt,column);
				ssep=sep;
			}
		}
		return s;
	}
	private string GetFormat(Hashtable dicFormat,IColumn column){
		string stype1=CSLAType(column);
		if(dicFormat.Contains(stype1))
			return (string)dicFormat[stype1];
		string stype2=column.DataTypeName;
		if(dicFormat.Contains(stype2))
			return (string)dicFormat[stype2];
		return string.Format("**** MyGeneration Error **** {0} of type {1},{2} not defined in Hashtable *",column.Name,stype1,stype2);
		//return null;
	}
	private string FormatColumn(Hashtable dicFormat,IColumn column,string sPrefix,string sAlias)
	{
		string sFmt = GetFormat(dicFormat,column);
		if(sFmt!=null)
		{
			return sPrefix + FormatColumn(sFmt,column,sAlias);
		}
		return "";
	}
	public string GetDescription(IColumn column)
	{
		string sDesc=column.Description;
		sDesc=sDesc.Replace("{datetime}","");
		sDesc=sDesc.Replace("{auto}","");
		sDesc=sDesc.Replace("{info}","");
		return sDesc;
	}


