﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.17929
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;



[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="BASE")]
public partial class Base_DataDataContext : System.Data.Linq.DataContext
{
	
	private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
	
  #region 擴充性方法定義
  partial void OnCreated();
    partial void InsertBRM_USERINFO(Base_Data.BRM_USERINFO instance);
    partial void UpdateBRM_USERINFO(Base_Data.BRM_USERINFO instance);
    partial void DeleteBRM_USERINFO(Base_Data.BRM_USERINFO instance);
  #endregion
	
	public Base_DataDataContext() : 
			base(global::System.Configuration.ConfigurationManager.ConnectionStrings["BASEConnectionString"].ConnectionString, mappingSource)
	{
		OnCreated();
	}
	
	public Base_DataDataContext(string connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public Base_DataDataContext(System.Data.IDbConnection connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public Base_DataDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public Base_DataDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public System.Data.Linq.Table<Base_Data.BRM_USERINFO> BRM_USERINFO
	{
		get
		{
			return this.GetTable<Base_Data.BRM_USERINFO>();
		}
	}
	
	public System.Data.Linq.Table<Base_Data.BRM_USER_INFO> BRM_USER_INFO
	{
		get
		{
			return this.GetTable<Base_Data.BRM_USER_INFO>();
		}
	}
}
namespace Base_Data
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.BRM_USERINFO")]
	public partial class BRM_USERINFO : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _ID;
		
		private string _UserID;
		
		private string _Password;
		
		private string _Name;
		
		private string _WorkStation;
		
		private string _SignLevel;
		
		private string _SignLevelDesc;
		
		private string _NotesID;
		
		private string _Mail;
		
		private string _Dept;
		
		private string _DeptCode;
		
		private System.Nullable<double> _DeptLevel;
		
		private string _Phone;
		
		private string _Mobile;
		
		private System.Nullable<char> _IsDeptManagerYN;
		
		private string _SuperiorID;
		
		private string _SuperiorName;
		
		private string _SuperiorNotesID;
		
		private string _SuperiorDeptName;
		
		private string _SuperiorDeptCode;
		
		private System.Nullable<char> _AgentTurnOnYN;
		
		private string _FirstAgentNo;
		
		private string _SecondAgentNo;
		
		private string _ThirdAgentNo;
		
		private string _FirstAgentNotesID;
		
		private string _SecondAgentNotesID;
		
		private string _ThirdAgentNotesID;
		
		private System.Nullable<char> _FlagOfLeftYN;
		
		private string _YearsOfInduction;
		
		private System.Nullable<double> _YearsOfOutside;
		
		private string _SalaryLevel;
		
		private System.Nullable<System.DateTime> _InductionDate;
		
		private System.Nullable<System.DateTime> _LeftDate;
		
		private System.Nullable<System.DateTime> _SyncDate;
		
    #region 擴充性方法定義
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnIDChanging(int value);
    partial void OnIDChanged();
    partial void OnUserIDChanging(string value);
    partial void OnUserIDChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnNameChanging(string value);
    partial void OnNameChanged();
    partial void OnWorkStationChanging(string value);
    partial void OnWorkStationChanged();
    partial void OnSignLevelChanging(string value);
    partial void OnSignLevelChanged();
    partial void OnSignLevelDescChanging(string value);
    partial void OnSignLevelDescChanged();
    partial void OnNotesIDChanging(string value);
    partial void OnNotesIDChanged();
    partial void OnMailChanging(string value);
    partial void OnMailChanged();
    partial void OnDeptChanging(string value);
    partial void OnDeptChanged();
    partial void OnDeptCodeChanging(string value);
    partial void OnDeptCodeChanged();
    partial void OnDeptLevelChanging(System.Nullable<double> value);
    partial void OnDeptLevelChanged();
    partial void OnPhoneChanging(string value);
    partial void OnPhoneChanged();
    partial void OnMobileChanging(string value);
    partial void OnMobileChanged();
    partial void OnIsDeptManagerYNChanging(System.Nullable<char> value);
    partial void OnIsDeptManagerYNChanged();
    partial void OnSuperiorIDChanging(string value);
    partial void OnSuperiorIDChanged();
    partial void OnSuperiorNameChanging(string value);
    partial void OnSuperiorNameChanged();
    partial void OnSuperiorNotesIDChanging(string value);
    partial void OnSuperiorNotesIDChanged();
    partial void OnSuperiorDeptNameChanging(string value);
    partial void OnSuperiorDeptNameChanged();
    partial void OnSuperiorDeptCodeChanging(string value);
    partial void OnSuperiorDeptCodeChanged();
    partial void OnAgentTurnOnYNChanging(System.Nullable<char> value);
    partial void OnAgentTurnOnYNChanged();
    partial void OnFirstAgentNoChanging(string value);
    partial void OnFirstAgentNoChanged();
    partial void OnSecondAgentNoChanging(string value);
    partial void OnSecondAgentNoChanged();
    partial void OnThirdAgentNoChanging(string value);
    partial void OnThirdAgentNoChanged();
    partial void OnFirstAgentNotesIDChanging(string value);
    partial void OnFirstAgentNotesIDChanged();
    partial void OnSecondAgentNotesIDChanging(string value);
    partial void OnSecondAgentNotesIDChanged();
    partial void OnThirdAgentNotesIDChanging(string value);
    partial void OnThirdAgentNotesIDChanged();
    partial void OnFlagOfLeftYNChanging(System.Nullable<char> value);
    partial void OnFlagOfLeftYNChanged();
    partial void OnYearsOfInductionChanging(string value);
    partial void OnYearsOfInductionChanged();
    partial void OnYearsOfOutsideChanging(System.Nullable<double> value);
    partial void OnYearsOfOutsideChanged();
    partial void OnSalaryLevelChanging(string value);
    partial void OnSalaryLevelChanged();
    partial void OnInductionDateChanging(System.Nullable<System.DateTime> value);
    partial void OnInductionDateChanged();
    partial void OnLeftDateChanging(System.Nullable<System.DateTime> value);
    partial void OnLeftDateChanged();
    partial void OnSyncDateChanging(System.Nullable<System.DateTime> value);
    partial void OnSyncDateChanged();
    #endregion
		
		public BRM_USERINFO()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int ID
		{
			get
			{
				return this._ID;
			}
			set
			{
				if ((this._ID != value))
				{
					this.OnIDChanging(value);
					this.SendPropertyChanging();
					this._ID = value;
					this.SendPropertyChanged("ID");
					this.OnIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this.OnUserIDChanging(value);
					this.SendPropertyChanging();
					this._UserID = value;
					this.SendPropertyChanged("UserID");
					this.OnUserIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Password", DbType="NVarChar(50)")]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="NVarChar(50)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this.OnNameChanging(value);
					this.SendPropertyChanging();
					this._Name = value;
					this.SendPropertyChanged("Name");
					this.OnNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WorkStation", DbType="NVarChar(255)")]
		public string WorkStation
		{
			get
			{
				return this._WorkStation;
			}
			set
			{
				if ((this._WorkStation != value))
				{
					this.OnWorkStationChanging(value);
					this.SendPropertyChanging();
					this._WorkStation = value;
					this.SendPropertyChanged("WorkStation");
					this.OnWorkStationChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SignLevel", DbType="NVarChar(255)")]
		public string SignLevel
		{
			get
			{
				return this._SignLevel;
			}
			set
			{
				if ((this._SignLevel != value))
				{
					this.OnSignLevelChanging(value);
					this.SendPropertyChanging();
					this._SignLevel = value;
					this.SendPropertyChanged("SignLevel");
					this.OnSignLevelChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SignLevelDesc", DbType="NVarChar(50)")]
		public string SignLevelDesc
		{
			get
			{
				return this._SignLevelDesc;
			}
			set
			{
				if ((this._SignLevelDesc != value))
				{
					this.OnSignLevelDescChanging(value);
					this.SendPropertyChanging();
					this._SignLevelDesc = value;
					this.SendPropertyChanged("SignLevelDesc");
					this.OnSignLevelDescChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NotesID", DbType="NVarChar(255)")]
		public string NotesID
		{
			get
			{
				return this._NotesID;
			}
			set
			{
				if ((this._NotesID != value))
				{
					this.OnNotesIDChanging(value);
					this.SendPropertyChanging();
					this._NotesID = value;
					this.SendPropertyChanged("NotesID");
					this.OnNotesIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Mail", DbType="NVarChar(50)")]
		public string Mail
		{
			get
			{
				return this._Mail;
			}
			set
			{
				if ((this._Mail != value))
				{
					this.OnMailChanging(value);
					this.SendPropertyChanging();
					this._Mail = value;
					this.SendPropertyChanged("Mail");
					this.OnMailChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dept", DbType="NVarChar(255)")]
		public string Dept
		{
			get
			{
				return this._Dept;
			}
			set
			{
				if ((this._Dept != value))
				{
					this.OnDeptChanging(value);
					this.SendPropertyChanging();
					this._Dept = value;
					this.SendPropertyChanged("Dept");
					this.OnDeptChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeptCode", DbType="NVarChar(255)")]
		public string DeptCode
		{
			get
			{
				return this._DeptCode;
			}
			set
			{
				if ((this._DeptCode != value))
				{
					this.OnDeptCodeChanging(value);
					this.SendPropertyChanging();
					this._DeptCode = value;
					this.SendPropertyChanged("DeptCode");
					this.OnDeptCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeptLevel", DbType="Float")]
		public System.Nullable<double> DeptLevel
		{
			get
			{
				return this._DeptLevel;
			}
			set
			{
				if ((this._DeptLevel != value))
				{
					this.OnDeptLevelChanging(value);
					this.SendPropertyChanging();
					this._DeptLevel = value;
					this.SendPropertyChanged("DeptLevel");
					this.OnDeptLevelChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Phone", DbType="VarChar(50)")]
		public string Phone
		{
			get
			{
				return this._Phone;
			}
			set
			{
				if ((this._Phone != value))
				{
					this.OnPhoneChanging(value);
					this.SendPropertyChanging();
					this._Phone = value;
					this.SendPropertyChanged("Phone");
					this.OnPhoneChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Mobile", DbType="VarChar(50)")]
		public string Mobile
		{
			get
			{
				return this._Mobile;
			}
			set
			{
				if ((this._Mobile != value))
				{
					this.OnMobileChanging(value);
					this.SendPropertyChanging();
					this._Mobile = value;
					this.SendPropertyChanged("Mobile");
					this.OnMobileChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsDeptManagerYN", DbType="Char(1)")]
		public System.Nullable<char> IsDeptManagerYN
		{
			get
			{
				return this._IsDeptManagerYN;
			}
			set
			{
				if ((this._IsDeptManagerYN != value))
				{
					this.OnIsDeptManagerYNChanging(value);
					this.SendPropertyChanging();
					this._IsDeptManagerYN = value;
					this.SendPropertyChanged("IsDeptManagerYN");
					this.OnIsDeptManagerYNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorID", DbType="NVarChar(255)")]
		public string SuperiorID
		{
			get
			{
				return this._SuperiorID;
			}
			set
			{
				if ((this._SuperiorID != value))
				{
					this.OnSuperiorIDChanging(value);
					this.SendPropertyChanging();
					this._SuperiorID = value;
					this.SendPropertyChanged("SuperiorID");
					this.OnSuperiorIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorName", DbType="NVarChar(255)")]
		public string SuperiorName
		{
			get
			{
				return this._SuperiorName;
			}
			set
			{
				if ((this._SuperiorName != value))
				{
					this.OnSuperiorNameChanging(value);
					this.SendPropertyChanging();
					this._SuperiorName = value;
					this.SendPropertyChanged("SuperiorName");
					this.OnSuperiorNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorNotesID", DbType="NVarChar(255)")]
		public string SuperiorNotesID
		{
			get
			{
				return this._SuperiorNotesID;
			}
			set
			{
				if ((this._SuperiorNotesID != value))
				{
					this.OnSuperiorNotesIDChanging(value);
					this.SendPropertyChanging();
					this._SuperiorNotesID = value;
					this.SendPropertyChanged("SuperiorNotesID");
					this.OnSuperiorNotesIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorDeptName", DbType="NVarChar(50)")]
		public string SuperiorDeptName
		{
			get
			{
				return this._SuperiorDeptName;
			}
			set
			{
				if ((this._SuperiorDeptName != value))
				{
					this.OnSuperiorDeptNameChanging(value);
					this.SendPropertyChanging();
					this._SuperiorDeptName = value;
					this.SendPropertyChanged("SuperiorDeptName");
					this.OnSuperiorDeptNameChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorDeptCode", DbType="NVarChar(50)")]
		public string SuperiorDeptCode
		{
			get
			{
				return this._SuperiorDeptCode;
			}
			set
			{
				if ((this._SuperiorDeptCode != value))
				{
					this.OnSuperiorDeptCodeChanging(value);
					this.SendPropertyChanging();
					this._SuperiorDeptCode = value;
					this.SendPropertyChanged("SuperiorDeptCode");
					this.OnSuperiorDeptCodeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AgentTurnOnYN", DbType="Char(1)")]
		public System.Nullable<char> AgentTurnOnYN
		{
			get
			{
				return this._AgentTurnOnYN;
			}
			set
			{
				if ((this._AgentTurnOnYN != value))
				{
					this.OnAgentTurnOnYNChanging(value);
					this.SendPropertyChanging();
					this._AgentTurnOnYN = value;
					this.SendPropertyChanged("AgentTurnOnYN");
					this.OnAgentTurnOnYNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstAgentNo", DbType="VarChar(50)")]
		public string FirstAgentNo
		{
			get
			{
				return this._FirstAgentNo;
			}
			set
			{
				if ((this._FirstAgentNo != value))
				{
					this.OnFirstAgentNoChanging(value);
					this.SendPropertyChanging();
					this._FirstAgentNo = value;
					this.SendPropertyChanged("FirstAgentNo");
					this.OnFirstAgentNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SecondAgentNo", DbType="VarChar(50)")]
		public string SecondAgentNo
		{
			get
			{
				return this._SecondAgentNo;
			}
			set
			{
				if ((this._SecondAgentNo != value))
				{
					this.OnSecondAgentNoChanging(value);
					this.SendPropertyChanging();
					this._SecondAgentNo = value;
					this.SendPropertyChanged("SecondAgentNo");
					this.OnSecondAgentNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ThirdAgentNo", DbType="VarChar(50)")]
		public string ThirdAgentNo
		{
			get
			{
				return this._ThirdAgentNo;
			}
			set
			{
				if ((this._ThirdAgentNo != value))
				{
					this.OnThirdAgentNoChanging(value);
					this.SendPropertyChanging();
					this._ThirdAgentNo = value;
					this.SendPropertyChanged("ThirdAgentNo");
					this.OnThirdAgentNoChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstAgentNotesID", DbType="NVarChar(255)")]
		public string FirstAgentNotesID
		{
			get
			{
				return this._FirstAgentNotesID;
			}
			set
			{
				if ((this._FirstAgentNotesID != value))
				{
					this.OnFirstAgentNotesIDChanging(value);
					this.SendPropertyChanging();
					this._FirstAgentNotesID = value;
					this.SendPropertyChanged("FirstAgentNotesID");
					this.OnFirstAgentNotesIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SecondAgentNotesID", DbType="NVarChar(255)")]
		public string SecondAgentNotesID
		{
			get
			{
				return this._SecondAgentNotesID;
			}
			set
			{
				if ((this._SecondAgentNotesID != value))
				{
					this.OnSecondAgentNotesIDChanging(value);
					this.SendPropertyChanging();
					this._SecondAgentNotesID = value;
					this.SendPropertyChanged("SecondAgentNotesID");
					this.OnSecondAgentNotesIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ThirdAgentNotesID", DbType="NVarChar(255)")]
		public string ThirdAgentNotesID
		{
			get
			{
				return this._ThirdAgentNotesID;
			}
			set
			{
				if ((this._ThirdAgentNotesID != value))
				{
					this.OnThirdAgentNotesIDChanging(value);
					this.SendPropertyChanging();
					this._ThirdAgentNotesID = value;
					this.SendPropertyChanged("ThirdAgentNotesID");
					this.OnThirdAgentNotesIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FlagOfLeftYN", DbType="Char(1)")]
		public System.Nullable<char> FlagOfLeftYN
		{
			get
			{
				return this._FlagOfLeftYN;
			}
			set
			{
				if ((this._FlagOfLeftYN != value))
				{
					this.OnFlagOfLeftYNChanging(value);
					this.SendPropertyChanging();
					this._FlagOfLeftYN = value;
					this.SendPropertyChanged("FlagOfLeftYN");
					this.OnFlagOfLeftYNChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_YearsOfInduction", DbType="NVarChar(255)")]
		public string YearsOfInduction
		{
			get
			{
				return this._YearsOfInduction;
			}
			set
			{
				if ((this._YearsOfInduction != value))
				{
					this.OnYearsOfInductionChanging(value);
					this.SendPropertyChanging();
					this._YearsOfInduction = value;
					this.SendPropertyChanged("YearsOfInduction");
					this.OnYearsOfInductionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_YearsOfOutside", DbType="Float")]
		public System.Nullable<double> YearsOfOutside
		{
			get
			{
				return this._YearsOfOutside;
			}
			set
			{
				if ((this._YearsOfOutside != value))
				{
					this.OnYearsOfOutsideChanging(value);
					this.SendPropertyChanging();
					this._YearsOfOutside = value;
					this.SendPropertyChanged("YearsOfOutside");
					this.OnYearsOfOutsideChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SalaryLevel", DbType="NVarChar(255)")]
		public string SalaryLevel
		{
			get
			{
				return this._SalaryLevel;
			}
			set
			{
				if ((this._SalaryLevel != value))
				{
					this.OnSalaryLevelChanging(value);
					this.SendPropertyChanging();
					this._SalaryLevel = value;
					this.SendPropertyChanged("SalaryLevel");
					this.OnSalaryLevelChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_InductionDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> InductionDate
		{
			get
			{
				return this._InductionDate;
			}
			set
			{
				if ((this._InductionDate != value))
				{
					this.OnInductionDateChanging(value);
					this.SendPropertyChanging();
					this._InductionDate = value;
					this.SendPropertyChanged("InductionDate");
					this.OnInductionDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_LeftDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> LeftDate
		{
			get
			{
				return this._LeftDate;
			}
			set
			{
				if ((this._LeftDate != value))
				{
					this.OnLeftDateChanging(value);
					this.SendPropertyChanging();
					this._LeftDate = value;
					this.SendPropertyChanged("LeftDate");
					this.OnLeftDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SyncDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> SyncDate
		{
			get
			{
				return this._SyncDate;
			}
			set
			{
				if ((this._SyncDate != value))
				{
					this.OnSyncDateChanging(value);
					this.SendPropertyChanging();
					this._SyncDate = value;
					this.SendPropertyChanged("SyncDate");
					this.OnSyncDateChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.BRM_USER_INFO")]
	public partial class BRM_USER_INFO
	{
		
		private string _UserID;
		
		private string _password;
		
		private string _Name;
		
		private string _WorkStation;
		
		private string _SignLevel;
		
		private string _SignLevelDesc;
		
		private string _Mail;
		
		private string _Dept;
		
		private string _DeptCode;
		
		private System.Nullable<int> _DeptLevel;
		
		private string _Phone;
		
		private string _IsDeptManagerYN;
		
		private string _SuperiorID;
		
		private string _SuperiorName;
		
		private string _SuperiorDeptName;
		
		private string _SuperiorDeptCode;
		
		private string _AgentTurnOnYN;
		
		private string _FlagOfLeftYN;
		
		private System.Nullable<int> _SalaryID;
		
		private string _SalaryLevel;
		
		private string _NotesID;
		
		private string _FirstAgentNo;
		
		private string _SecondAgentNo;
		
		private string _ThirdAgentNo;
		
		private string _CompanyName;
		
		public BRM_USER_INFO()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string UserID
		{
			get
			{
				return this._UserID;
			}
			set
			{
				if ((this._UserID != value))
				{
					this._UserID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_password", DbType="VarChar(10) NOT NULL", CanBeNull=false)]
		public string password
		{
			get
			{
				return this._password;
			}
			set
			{
				if ((this._password != value))
				{
					this._password = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Name", DbType="VarChar(30)")]
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				if ((this._Name != value))
				{
					this._Name = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_WorkStation", DbType="VarChar(30)")]
		public string WorkStation
		{
			get
			{
				return this._WorkStation;
			}
			set
			{
				if ((this._WorkStation != value))
				{
					this._WorkStation = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SignLevel", DbType="VarChar(3)")]
		public string SignLevel
		{
			get
			{
				return this._SignLevel;
			}
			set
			{
				if ((this._SignLevel != value))
				{
					this._SignLevel = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SignLevelDesc", DbType="VarChar(100)")]
		public string SignLevelDesc
		{
			get
			{
				return this._SignLevelDesc;
			}
			set
			{
				if ((this._SignLevelDesc != value))
				{
					this._SignLevelDesc = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Mail", DbType="VarChar(25) NOT NULL", CanBeNull=false)]
		public string Mail
		{
			get
			{
				return this._Mail;
			}
			set
			{
				if ((this._Mail != value))
				{
					this._Mail = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Dept", DbType="VarChar(100)")]
		public string Dept
		{
			get
			{
				return this._Dept;
			}
			set
			{
				if ((this._Dept != value))
				{
					this._Dept = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeptCode", DbType="VarChar(10)")]
		public string DeptCode
		{
			get
			{
				return this._DeptCode;
			}
			set
			{
				if ((this._DeptCode != value))
				{
					this._DeptCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_DeptLevel", DbType="Int")]
		public System.Nullable<int> DeptLevel
		{
			get
			{
				return this._DeptLevel;
			}
			set
			{
				if ((this._DeptLevel != value))
				{
					this._DeptLevel = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Phone", DbType="VarChar(50)")]
		public string Phone
		{
			get
			{
				return this._Phone;
			}
			set
			{
				if ((this._Phone != value))
				{
					this._Phone = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_IsDeptManagerYN", DbType="VarChar(1)")]
		public string IsDeptManagerYN
		{
			get
			{
				return this._IsDeptManagerYN;
			}
			set
			{
				if ((this._IsDeptManagerYN != value))
				{
					this._IsDeptManagerYN = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorID", DbType="VarChar(50)")]
		public string SuperiorID
		{
			get
			{
				return this._SuperiorID;
			}
			set
			{
				if ((this._SuperiorID != value))
				{
					this._SuperiorID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorName", DbType="VarChar(100)")]
		public string SuperiorName
		{
			get
			{
				return this._SuperiorName;
			}
			set
			{
				if ((this._SuperiorName != value))
				{
					this._SuperiorName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorDeptName", DbType="VarChar(50)")]
		public string SuperiorDeptName
		{
			get
			{
				return this._SuperiorDeptName;
			}
			set
			{
				if ((this._SuperiorDeptName != value))
				{
					this._SuperiorDeptName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SuperiorDeptCode", DbType="VarChar(10)")]
		public string SuperiorDeptCode
		{
			get
			{
				return this._SuperiorDeptCode;
			}
			set
			{
				if ((this._SuperiorDeptCode != value))
				{
					this._SuperiorDeptCode = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_AgentTurnOnYN", DbType="VarChar(1) NOT NULL", CanBeNull=false)]
		public string AgentTurnOnYN
		{
			get
			{
				return this._AgentTurnOnYN;
			}
			set
			{
				if ((this._AgentTurnOnYN != value))
				{
					this._AgentTurnOnYN = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FlagOfLeftYN", DbType="VarChar(1) NOT NULL", CanBeNull=false)]
		public string FlagOfLeftYN
		{
			get
			{
				return this._FlagOfLeftYN;
			}
			set
			{
				if ((this._FlagOfLeftYN != value))
				{
					this._FlagOfLeftYN = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SalaryID", DbType="Int")]
		public System.Nullable<int> SalaryID
		{
			get
			{
				return this._SalaryID;
			}
			set
			{
				if ((this._SalaryID != value))
				{
					this._SalaryID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SalaryLevel", DbType="VarChar(30)")]
		public string SalaryLevel
		{
			get
			{
				return this._SalaryLevel;
			}
			set
			{
				if ((this._SalaryLevel != value))
				{
					this._SalaryLevel = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_NotesID", DbType="VarChar(50)")]
		public string NotesID
		{
			get
			{
				return this._NotesID;
			}
			set
			{
				if ((this._NotesID != value))
				{
					this._NotesID = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_FirstAgentNo", DbType="VarChar(30)")]
		public string FirstAgentNo
		{
			get
			{
				return this._FirstAgentNo;
			}
			set
			{
				if ((this._FirstAgentNo != value))
				{
					this._FirstAgentNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SecondAgentNo", DbType="VarChar(30)")]
		public string SecondAgentNo
		{
			get
			{
				return this._SecondAgentNo;
			}
			set
			{
				if ((this._SecondAgentNo != value))
				{
					this._SecondAgentNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ThirdAgentNo", DbType="VarChar(30)")]
		public string ThirdAgentNo
		{
			get
			{
				return this._ThirdAgentNo;
			}
			set
			{
				if ((this._ThirdAgentNo != value))
				{
					this._ThirdAgentNo = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CompanyName", DbType="VarChar(50)")]
		public string CompanyName
		{
			get
			{
				return this._CompanyName;
			}
			set
			{
				if ((this._CompanyName != value))
				{
					this._CompanyName = value;
				}
			}
		}
	}
}
#pragma warning restore 1591
