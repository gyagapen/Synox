﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.239
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ServiceSMS
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[global::System.Data.Linq.Mapping.DatabaseAttribute(Name="SMS_DB")]
	public partial class DBSMSContextDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Définitions de méthodes d'extensibilité
    partial void OnCreated();
    partial void InsertEncodage(Encodage instance);
    partial void UpdateEncodage(Encodage instance);
    partial void DeleteEncodage(Encodage instance);
    partial void InsertMessage(Message instance);
    partial void UpdateMessage(Message instance);
    partial void DeleteMessage(Message instance);
    partial void InsertMessageRecu(MessageRecu instance);
    partial void UpdateMessageRecu(MessageRecu instance);
    partial void DeleteMessageRecu(MessageRecu instance);
    partial void InsertStatut(Statut instance);
    partial void UpdateStatut(Statut instance);
    partial void DeleteStatut(Statut instance);
    partial void InsertMessageEnvoi(MessageEnvoi instance);
    partial void UpdateMessageEnvoi(MessageEnvoi instance);
    partial void DeleteMessageEnvoi(MessageEnvoi instance);
    #endregion
		
		public DBSMSContextDataContext() : 
				base(global::ServiceSMS.Properties.Settings.Default.SMS_DBConnectionString2, mappingSource)
		{
			OnCreated();
		}
		
		public DBSMSContextDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBSMSContextDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBSMSContextDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public DBSMSContextDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<Encodage> Encodage
		{
			get
			{
				return this.GetTable<Encodage>();
			}
		}
		
		public System.Data.Linq.Table<Message> Message
		{
			get
			{
				return this.GetTable<Message>();
			}
		}
		
		public System.Data.Linq.Table<MessageRecu> MessageRecu
		{
			get
			{
				return this.GetTable<MessageRecu>();
			}
		}
		
		public System.Data.Linq.Table<Statut> Statut
		{
			get
			{
				return this.GetTable<Statut>();
			}
		}
		
		public System.Data.Linq.Table<MessageEnvoi> MessageEnvoi
		{
			get
			{
				return this.GetTable<MessageEnvoi>();
			}
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Encodage")]
	public partial class Encodage : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idEncodage;
		
		private string _libelleEncodage;
		
		private EntitySet<Message> _Message;
		
    #region Définitions de méthodes d'extensibilité
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidEncodageChanging(int value);
    partial void OnidEncodageChanged();
    partial void OnlibelleEncodageChanging(string value);
    partial void OnlibelleEncodageChanged();
    #endregion
		
		public Encodage()
		{
			this._Message = new EntitySet<Message>(new Action<Message>(this.attach_Message), new Action<Message>(this.detach_Message));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idEncodage", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int idEncodage
		{
			get
			{
				return this._idEncodage;
			}
			set
			{
				if ((this._idEncodage != value))
				{
					this.OnidEncodageChanging(value);
					this.SendPropertyChanging();
					this._idEncodage = value;
					this.SendPropertyChanged("idEncodage");
					this.OnidEncodageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_libelleEncodage", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string libelleEncodage
		{
			get
			{
				return this._libelleEncodage;
			}
			set
			{
				if ((this._libelleEncodage != value))
				{
					this.OnlibelleEncodageChanging(value);
					this.SendPropertyChanging();
					this._libelleEncodage = value;
					this.SendPropertyChanged("libelleEncodage");
					this.OnlibelleEncodageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Encodage_Message", Storage="_Message", ThisKey="idEncodage", OtherKey="idEncodage")]
		public EntitySet<Message> Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message.Assign(value);
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
		
		private void attach_Message(Message entity)
		{
			this.SendPropertyChanging();
			entity.Encodage = this;
		}
		
		private void detach_Message(Message entity)
		{
			this.SendPropertyChanging();
			entity.Encodage = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Message")]
	public partial class Message : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idMessage;
		
		private string _messageTexte;
		
		private string _messagePDU;
		
		private int _accuseReception;
		
		private string _noDestinataire;
		
		private string _noEmetteur;
		
		private int _idEncodage;
		
		private int _idStatut;
		
		private EntityRef<MessageRecu> _MessageRecu;
		
		private EntityRef<MessageEnvoi> _MessageEnvoi;
		
		private EntityRef<Encodage> _Encodage;
		
		private EntityRef<Statut> _Statut;
		
    #region Définitions de méthodes d'extensibilité
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidMessageChanging(int value);
    partial void OnidMessageChanged();
    partial void OnmessageTexteChanging(string value);
    partial void OnmessageTexteChanged();
    partial void OnmessagePDUChanging(string value);
    partial void OnmessagePDUChanged();
    partial void OnaccuseReceptionChanging(int value);
    partial void OnaccuseReceptionChanged();
    partial void OnnoDestinataireChanging(string value);
    partial void OnnoDestinataireChanged();
    partial void OnnoEmetteurChanging(string value);
    partial void OnnoEmetteurChanged();
    partial void OnidEncodageChanging(int value);
    partial void OnidEncodageChanged();
    partial void OnidStatutChanging(int value);
    partial void OnidStatutChanged();
    #endregion
		
		public Message()
		{
			this._MessageRecu = default(EntityRef<MessageRecu>);
			this._MessageEnvoi = default(EntityRef<MessageEnvoi>);
			this._Encodage = default(EntityRef<Encodage>);
			this._Statut = default(EntityRef<Statut>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idMessage", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int idMessage
		{
			get
			{
				return this._idMessage;
			}
			set
			{
				if ((this._idMessage != value))
				{
					this.OnidMessageChanging(value);
					this.SendPropertyChanging();
					this._idMessage = value;
					this.SendPropertyChanged("idMessage");
					this.OnidMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_messageTexte", DbType="NVarChar(MAX)")]
		public string messageTexte
		{
			get
			{
				return this._messageTexte;
			}
			set
			{
				if ((this._messageTexte != value))
				{
					this.OnmessageTexteChanging(value);
					this.SendPropertyChanging();
					this._messageTexte = value;
					this.SendPropertyChanged("messageTexte");
					this.OnmessageTexteChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_messagePDU", DbType="NVarChar(MAX)")]
		public string messagePDU
		{
			get
			{
				return this._messagePDU;
			}
			set
			{
				if ((this._messagePDU != value))
				{
					this.OnmessagePDUChanging(value);
					this.SendPropertyChanging();
					this._messagePDU = value;
					this.SendPropertyChanged("messagePDU");
					this.OnmessagePDUChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_accuseReception", DbType="Int NOT NULL")]
		public int accuseReception
		{
			get
			{
				return this._accuseReception;
			}
			set
			{
				if ((this._accuseReception != value))
				{
					this.OnaccuseReceptionChanging(value);
					this.SendPropertyChanging();
					this._accuseReception = value;
					this.SendPropertyChanged("accuseReception");
					this.OnaccuseReceptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_noDestinataire", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string noDestinataire
		{
			get
			{
				return this._noDestinataire;
			}
			set
			{
				if ((this._noDestinataire != value))
				{
					this.OnnoDestinataireChanging(value);
					this.SendPropertyChanging();
					this._noDestinataire = value;
					this.SendPropertyChanged("noDestinataire");
					this.OnnoDestinataireChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_noEmetteur", DbType="NVarChar(50)")]
		public string noEmetteur
		{
			get
			{
				return this._noEmetteur;
			}
			set
			{
				if ((this._noEmetteur != value))
				{
					this.OnnoEmetteurChanging(value);
					this.SendPropertyChanging();
					this._noEmetteur = value;
					this.SendPropertyChanged("noEmetteur");
					this.OnnoEmetteurChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idEncodage", DbType="Int NOT NULL")]
		public int idEncodage
		{
			get
			{
				return this._idEncodage;
			}
			set
			{
				if ((this._idEncodage != value))
				{
					if (this._Encodage.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnidEncodageChanging(value);
					this.SendPropertyChanging();
					this._idEncodage = value;
					this.SendPropertyChanged("idEncodage");
					this.OnidEncodageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idStatut", DbType="Int NOT NULL")]
		public int idStatut
		{
			get
			{
				return this._idStatut;
			}
			set
			{
				if ((this._idStatut != value))
				{
					if (this._Statut.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnidStatutChanging(value);
					this.SendPropertyChanging();
					this._idStatut = value;
					this.SendPropertyChanged("idStatut");
					this.OnidStatutChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Message_MessageRecu", Storage="_MessageRecu", ThisKey="idMessage", OtherKey="idMessage", IsUnique=true, IsForeignKey=false)]
		public MessageRecu MessageRecu
		{
			get
			{
				return this._MessageRecu.Entity;
			}
			set
			{
				MessageRecu previousValue = this._MessageRecu.Entity;
				if (((previousValue != value) 
							|| (this._MessageRecu.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._MessageRecu.Entity = null;
						previousValue.Message = null;
					}
					this._MessageRecu.Entity = value;
					if ((value != null))
					{
						value.Message = this;
					}
					this.SendPropertyChanged("MessageRecu");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Message_MessageEnvoi", Storage="_MessageEnvoi", ThisKey="idMessage", OtherKey="idMessage", IsUnique=true, IsForeignKey=false)]
		public MessageEnvoi MessageEnvoi
		{
			get
			{
				return this._MessageEnvoi.Entity;
			}
			set
			{
				MessageEnvoi previousValue = this._MessageEnvoi.Entity;
				if (((previousValue != value) 
							|| (this._MessageEnvoi.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._MessageEnvoi.Entity = null;
						previousValue.Message = null;
					}
					this._MessageEnvoi.Entity = value;
					if ((value != null))
					{
						value.Message = this;
					}
					this.SendPropertyChanged("MessageEnvoi");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Encodage_Message", Storage="_Encodage", ThisKey="idEncodage", OtherKey="idEncodage", IsForeignKey=true)]
		public Encodage Encodage
		{
			get
			{
				return this._Encodage.Entity;
			}
			set
			{
				Encodage previousValue = this._Encodage.Entity;
				if (((previousValue != value) 
							|| (this._Encodage.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Encodage.Entity = null;
						previousValue.Message.Remove(this);
					}
					this._Encodage.Entity = value;
					if ((value != null))
					{
						value.Message.Add(this);
						this._idEncodage = value.idEncodage;
					}
					else
					{
						this._idEncodage = default(int);
					}
					this.SendPropertyChanged("Encodage");
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Statut_Message", Storage="_Statut", ThisKey="idStatut", OtherKey="idStatut", IsForeignKey=true)]
		public Statut Statut
		{
			get
			{
				return this._Statut.Entity;
			}
			set
			{
				Statut previousValue = this._Statut.Entity;
				if (((previousValue != value) 
							|| (this._Statut.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Statut.Entity = null;
						previousValue.Message.Remove(this);
					}
					this._Statut.Entity = value;
					if ((value != null))
					{
						value.Message.Add(this);
						this._idStatut = value.idStatut;
					}
					else
					{
						this._idStatut = default(int);
					}
					this.SendPropertyChanged("Statut");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MessageRecu")]
	public partial class MessageRecu : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idMessage;
		
		private System.DateTime _dateReception;
		
		private System.Nullable<System.DateTime> _dateLecture;
		
		private EntityRef<Message> _Message;
		
    #region Définitions de méthodes d'extensibilité
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidMessageChanging(int value);
    partial void OnidMessageChanged();
    partial void OndateReceptionChanging(System.DateTime value);
    partial void OndateReceptionChanged();
    partial void OndateLectureChanging(System.Nullable<System.DateTime> value);
    partial void OndateLectureChanged();
    #endregion
		
		public MessageRecu()
		{
			this._Message = default(EntityRef<Message>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idMessage", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int idMessage
		{
			get
			{
				return this._idMessage;
			}
			set
			{
				if ((this._idMessage != value))
				{
					if (this._Message.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnidMessageChanging(value);
					this.SendPropertyChanging();
					this._idMessage = value;
					this.SendPropertyChanged("idMessage");
					this.OnidMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dateReception", DbType="Date NOT NULL")]
		public System.DateTime dateReception
		{
			get
			{
				return this._dateReception;
			}
			set
			{
				if ((this._dateReception != value))
				{
					this.OndateReceptionChanging(value);
					this.SendPropertyChanging();
					this._dateReception = value;
					this.SendPropertyChanged("dateReception");
					this.OndateReceptionChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dateLecture", DbType="Date")]
		public System.Nullable<System.DateTime> dateLecture
		{
			get
			{
				return this._dateLecture;
			}
			set
			{
				if ((this._dateLecture != value))
				{
					this.OndateLectureChanging(value);
					this.SendPropertyChanging();
					this._dateLecture = value;
					this.SendPropertyChanged("dateLecture");
					this.OndateLectureChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Message_MessageRecu", Storage="_Message", ThisKey="idMessage", OtherKey="idMessage", IsForeignKey=true)]
		public Message Message
		{
			get
			{
				return this._Message.Entity;
			}
			set
			{
				Message previousValue = this._Message.Entity;
				if (((previousValue != value) 
							|| (this._Message.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Message.Entity = null;
						previousValue.MessageRecu = null;
					}
					this._Message.Entity = value;
					if ((value != null))
					{
						value.MessageRecu = this;
						this._idMessage = value.idMessage;
					}
					else
					{
						this._idMessage = default(int);
					}
					this.SendPropertyChanged("Message");
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
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.Statut")]
	public partial class Statut : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idStatut;
		
		private string _libelleStatut;
		
		private EntitySet<Message> _Message;
		
    #region Définitions de méthodes d'extensibilité
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidStatutChanging(int value);
    partial void OnidStatutChanged();
    partial void OnlibelleStatutChanging(string value);
    partial void OnlibelleStatutChanged();
    #endregion
		
		public Statut()
		{
			this._Message = new EntitySet<Message>(new Action<Message>(this.attach_Message), new Action<Message>(this.detach_Message));
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idStatut", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int idStatut
		{
			get
			{
				return this._idStatut;
			}
			set
			{
				if ((this._idStatut != value))
				{
					this.OnidStatutChanging(value);
					this.SendPropertyChanging();
					this._idStatut = value;
					this.SendPropertyChanged("idStatut");
					this.OnidStatutChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_libelleStatut", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
		public string libelleStatut
		{
			get
			{
				return this._libelleStatut;
			}
			set
			{
				if ((this._libelleStatut != value))
				{
					this.OnlibelleStatutChanging(value);
					this.SendPropertyChanging();
					this._libelleStatut = value;
					this.SendPropertyChanged("libelleStatut");
					this.OnlibelleStatutChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Statut_Message", Storage="_Message", ThisKey="idStatut", OtherKey="idStatut")]
		public EntitySet<Message> Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				this._Message.Assign(value);
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
		
		private void attach_Message(Message entity)
		{
			this.SendPropertyChanging();
			entity.Statut = this;
		}
		
		private void detach_Message(Message entity)
		{
			this.SendPropertyChanging();
			entity.Statut = null;
		}
	}
	
	[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.MessageEnvoi")]
	public partial class MessageEnvoi : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _idMessage;
		
		private System.Nullable<System.DateTime> _dateDemande;
		
		private System.Nullable<System.DateTime> _dateEnvoi;
		
		private System.Nullable<int> _accuseReceptionRecu;
		
		private System.Nullable<System.DateTime> _dateReceptionAccuse;
		
		private System.Nullable<int> _dureeValidite;
		
		private string _referenceEnvoi;
		
		private EntityRef<Message> _Message;
		
    #region Définitions de méthodes d'extensibilité
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidMessageChanging(int value);
    partial void OnidMessageChanged();
    partial void OndateDemandeChanging(System.Nullable<System.DateTime> value);
    partial void OndateDemandeChanged();
    partial void OndateEnvoiChanging(System.Nullable<System.DateTime> value);
    partial void OndateEnvoiChanged();
    partial void OnaccuseReceptionRecuChanging(System.Nullable<int> value);
    partial void OnaccuseReceptionRecuChanged();
    partial void OndateReceptionAccuseChanging(System.Nullable<System.DateTime> value);
    partial void OndateReceptionAccuseChanged();
    partial void OndureeValiditeChanging(System.Nullable<int> value);
    partial void OndureeValiditeChanged();
    partial void OnreferenceEnvoiChanging(string value);
    partial void OnreferenceEnvoiChanged();
    #endregion
		
		public MessageEnvoi()
		{
			this._Message = default(EntityRef<Message>);
			OnCreated();
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_idMessage", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int idMessage
		{
			get
			{
				return this._idMessage;
			}
			set
			{
				if ((this._idMessage != value))
				{
					if (this._Message.HasLoadedOrAssignedValue)
					{
						throw new System.Data.Linq.ForeignKeyReferenceAlreadyHasValueException();
					}
					this.OnidMessageChanging(value);
					this.SendPropertyChanging();
					this._idMessage = value;
					this.SendPropertyChanged("idMessage");
					this.OnidMessageChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dateDemande", DbType="Date")]
		public System.Nullable<System.DateTime> dateDemande
		{
			get
			{
				return this._dateDemande;
			}
			set
			{
				if ((this._dateDemande != value))
				{
					this.OndateDemandeChanging(value);
					this.SendPropertyChanging();
					this._dateDemande = value;
					this.SendPropertyChanged("dateDemande");
					this.OndateDemandeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dateEnvoi", DbType="Date")]
		public System.Nullable<System.DateTime> dateEnvoi
		{
			get
			{
				return this._dateEnvoi;
			}
			set
			{
				if ((this._dateEnvoi != value))
				{
					this.OndateEnvoiChanging(value);
					this.SendPropertyChanging();
					this._dateEnvoi = value;
					this.SendPropertyChanged("dateEnvoi");
					this.OndateEnvoiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_accuseReceptionRecu", DbType="Int")]
		public System.Nullable<int> accuseReceptionRecu
		{
			get
			{
				return this._accuseReceptionRecu;
			}
			set
			{
				if ((this._accuseReceptionRecu != value))
				{
					this.OnaccuseReceptionRecuChanging(value);
					this.SendPropertyChanging();
					this._accuseReceptionRecu = value;
					this.SendPropertyChanged("accuseReceptionRecu");
					this.OnaccuseReceptionRecuChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dateReceptionAccuse", DbType="Date")]
		public System.Nullable<System.DateTime> dateReceptionAccuse
		{
			get
			{
				return this._dateReceptionAccuse;
			}
			set
			{
				if ((this._dateReceptionAccuse != value))
				{
					this.OndateReceptionAccuseChanging(value);
					this.SendPropertyChanging();
					this._dateReceptionAccuse = value;
					this.SendPropertyChanged("dateReceptionAccuse");
					this.OndateReceptionAccuseChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_dureeValidite", DbType="Int")]
		public System.Nullable<int> dureeValidite
		{
			get
			{
				return this._dureeValidite;
			}
			set
			{
				if ((this._dureeValidite != value))
				{
					this.OndureeValiditeChanging(value);
					this.SendPropertyChanging();
					this._dureeValidite = value;
					this.SendPropertyChanged("dureeValidite");
					this.OndureeValiditeChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_referenceEnvoi", DbType="NChar(10)")]
		public string referenceEnvoi
		{
			get
			{
				return this._referenceEnvoi;
			}
			set
			{
				if ((this._referenceEnvoi != value))
				{
					this.OnreferenceEnvoiChanging(value);
					this.SendPropertyChanging();
					this._referenceEnvoi = value;
					this.SendPropertyChanged("referenceEnvoi");
					this.OnreferenceEnvoiChanged();
				}
			}
		}
		
		[global::System.Data.Linq.Mapping.AssociationAttribute(Name="Message_MessageEnvoi", Storage="_Message", ThisKey="idMessage", OtherKey="idMessage", IsForeignKey=true)]
		public Message Message
		{
			get
			{
				return this._Message.Entity;
			}
			set
			{
				Message previousValue = this._Message.Entity;
				if (((previousValue != value) 
							|| (this._Message.HasLoadedOrAssignedValue == false)))
				{
					this.SendPropertyChanging();
					if ((previousValue != null))
					{
						this._Message.Entity = null;
						previousValue.MessageEnvoi = null;
					}
					this._Message.Entity = value;
					if ((value != null))
					{
						value.MessageEnvoi = this;
						this._idMessage = value.idMessage;
					}
					else
					{
						this._idMessage = default(int);
					}
					this.SendPropertyChanged("Message");
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
