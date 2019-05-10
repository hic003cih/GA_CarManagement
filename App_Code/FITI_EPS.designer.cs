﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
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
public partial class FITI_EPSDataContext : System.Data.Linq.DataContext
{
	
	private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
	
  #region 擴充性方法定義
  partial void OnCreated();
    partial void InsertBRM_USER_ROLE(FITI_EPS.BRM_USER_ROLE instance);
    partial void UpdateBRM_USER_ROLE(FITI_EPS.BRM_USER_ROLE instance);
    partial void DeleteBRM_USER_ROLE(FITI_EPS.BRM_USER_ROLE instance);
  #endregion
	
	public FITI_EPSDataContext() : 
			base(global::System.Configuration.ConfigurationManager.ConnectionStrings["BASEConnectionString"].ConnectionString, mappingSource)
	{
		OnCreated();
	}
	
	public FITI_EPSDataContext(string connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public FITI_EPSDataContext(System.Data.IDbConnection connection) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public FITI_EPSDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public FITI_EPSDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
			base(connection, mappingSource)
	{
		OnCreated();
	}
	
	public System.Data.Linq.Table<FITI_EPS.V_EPS_UserRoleAuthority> V_EPS_UserRoleAuthority
	{
		get
		{
			return this.GetTable<FITI_EPS.V_EPS_UserRoleAuthority>();
		}
	}
	
	public System.Data.Linq.Table<FITI_EPS.BRM_USER_ROLE> BRM_USER_ROLE
	{
		get
		{
			return this.GetTable<FITI_EPS.BRM_USER_ROLE>();
		}
	}
}
namespace FITI_EPS
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.V_EPS_UserRoleAuthority")]
	public partial class V_EPS_UserRoleAuthority
	{
		
		private string _UserID;
		
		private string _RoleName;
		
		private string _SystemName;
		
		private string _PageName;
		
		private string _ElementName;
		
		public V_EPS_UserRoleAuthority()
		{
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RoleName", DbType="NVarChar(50)")]
		public string RoleName
		{
			get
			{
				return this._RoleName;
			}
			set
			{
				if ((this._RoleName != value))
				{
					this._RoleName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_SystemName", DbType="VarChar(20)")]
		public string SystemName
		{
			get
			{
				return this._SystemName;
			}
			set
			{
				if ((this._SystemName != value))
				{
					this._SystemName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_PageName", DbType="VarChar(50)")]
		public string PageName
		{
			get
			{
				return this._PageName;
			}
			set
			{
				if ((this._PageName != value))
				{
					this._PageName = value;
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_ElementName", DbType="VarChar(100)")]
		public string ElementName
		{
			get
			{
				return this._ElementName;
			}
			set
			{
				if ((this._ElementName != value))
				{
					this._ElementName = value;
				}
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.BRM_USER_ROLE")]
	public partial class BRM_USER_ROLE : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private string _Creator;
		
		private string _CreateDate;
		
		private string _Modifier;
		
		private string _Modidate;
		
		private int _UserRoleID;
		
		private string _UserID;
		
		private System.Nullable<int> _RoleID;
		
		private string _EffectiveYN;
		
    #region 擴充性方法定義
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCreatorChanging(string value);
    partial void OnCreatorChanged();
    partial void OnCreateDateChanging(string value);
    partial void OnCreateDateChanged();
    partial void OnModifierChanging(string value);
    partial void OnModifierChanged();
    partial void OnModidateChanging(string value);
    partial void OnModidateChanged();
    partial void OnUserRoleIDChanging(int value);
    partial void OnUserRoleIDChanged();
    partial void OnUserIDChanging(string value);
    partial void OnUserIDChanged();
    partial void OnRoleIDChanging(System.Nullable<int> value);
    partial void OnRoleIDChanged();
    partial void OnEffectiveYNChanging(string value);
    partial void OnEffectiveYNChanged();
    #endregion
		
		public BRM_USER_ROLE()
		{
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Creator", DbType="VarChar(20)")]
		public string Creator
		{
			get
			{
				return this._Creator;
			}
			set
			{
				if ((this._Creator != value))
				{
					this.OnCreatorChanging(value);
					this.SendPropertyChanging();
					this._Creator = value;
					this.SendPropertyChanged("Creator");
					this.OnCreatorChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_CreateDate", DbType="VarChar(20)")]
		public string CreateDate
		{
			get
			{
				return this._CreateDate;
			}
			set
			{
				if ((this._CreateDate != value))
				{
					this.OnCreateDateChanging(value);
					this.SendPropertyChanging();
					this._CreateDate = value;
					this.SendPropertyChanged("CreateDate");
					this.OnCreateDateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Modifier", DbType="VarChar(20)")]
		public string Modifier
		{
			get
			{
				return this._Modifier;
			}
			set
			{
				if ((this._Modifier != value))
				{
					this.OnModifierChanging(value);
					this.SendPropertyChanging();
					this._Modifier = value;
					this.SendPropertyChanged("Modifier");
					this.OnModifierChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Modidate", DbType="VarChar(20)")]
		public string Modidate
		{
			get
			{
				return this._Modidate;
			}
			set
			{
				if ((this._Modidate != value))
				{
					this.OnModidateChanging(value);
					this.SendPropertyChanging();
					this._Modidate = value;
					this.SendPropertyChanged("Modidate");
					this.OnModidateChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserRoleID", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int UserRoleID
		{
			get
			{
				return this._UserRoleID;
			}
			set
			{
				if ((this._UserRoleID != value))
				{
					this.OnUserRoleIDChanging(value);
					this.SendPropertyChanging();
					this._UserRoleID = value;
					this.SendPropertyChanged("UserRoleID");
					this.OnUserRoleIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserID", DbType="VarChar(20) NOT NULL", CanBeNull=false)]
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
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_RoleID", DbType="Int")]
		public System.Nullable<int> RoleID
		{
			get
			{
				return this._RoleID;
			}
			set
			{
				if ((this._RoleID != value))
				{
					this.OnRoleIDChanging(value);
					this.SendPropertyChanging();
					this._RoleID = value;
					this.SendPropertyChanged("RoleID");
					this.OnRoleIDChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_EffectiveYN", DbType="VarChar(1) NOT NULL", CanBeNull=false)]
		public string EffectiveYN
		{
			get
			{
				return this._EffectiveYN;
			}
			set
			{
				if ((this._EffectiveYN != value))
				{
					this.OnEffectiveYNChanging(value);
					this.SendPropertyChanging();
					this._EffectiveYN = value;
					this.SendPropertyChanged("EffectiveYN");
					this.OnEffectiveYNChanged();
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
}
#pragma warning restore 1591