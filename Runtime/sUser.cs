namespace BimServerExchange.Runtime
{
	/// <opmerkingen/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
	[System.SerializableAttribute()]
	[System.Diagnostics.DebuggerStepThroughAttribute()]
	[System.ComponentModel.DesignerCategoryAttribute("code")]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "org.bimserver")]
	public partial class sUser
	{

		private long createdByIdField;

		private System.DateTime createdOnField;

		private bool createdOnFieldSpecified;

		private System.Nullable<long>[] extendedDataField;

		private System.Nullable<long>[] hasRightsOnField;

		private System.DateTime lastSeenField;

		private bool lastSeenFieldSpecified;

		private System.Nullable<long>[] logsField;

		private string nameField;

		private System.Nullable<long>[] oAuthAuthorizationCodesField;

		private System.Nullable<long>[] oAuthIssuedAuthorizationCodesField;

		private long oidField;

		private byte[] passwordHashField;

		private byte[] passwordSaltField;

		private System.Nullable<long>[] revisionsField;

		private int ridField;

		private System.Nullable<long>[] schemasField;

		private System.Nullable<long>[] servicesField;

		private sObjectState stateField;

		private bool stateFieldSpecified;

		private string tokenField;

		private long userSettingsIdField;

		private sUserType userTypeField;

		private bool userTypeFieldSpecified;

		private string usernameField;

		private byte[] validationTokenField;

		private System.DateTime validationTokenCreatedField;

		private bool validationTokenCreatedFieldSpecified;

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public long createdById
		{
			get
			{
				return this.createdByIdField;
			}
			set
			{
				this.createdByIdField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public System.DateTime createdOn
		{
			get
			{
				return this.createdOnField;
			}
			set
			{
				this.createdOnField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool createdOnSpecified
		{
			get
			{
				return this.createdOnFieldSpecified;
			}
			set
			{
				this.createdOnFieldSpecified = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("extendedData", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] extendedData
		{
			get
			{
				return this.extendedDataField;
			}
			set
			{
				this.extendedDataField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("hasRightsOn", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] hasRightsOn
		{
			get
			{
				return this.hasRightsOnField;
			}
			set
			{
				this.hasRightsOnField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public System.DateTime lastSeen
		{
			get
			{
				return this.lastSeenField;
			}
			set
			{
				this.lastSeenField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool lastSeenSpecified
		{
			get
			{
				return this.lastSeenFieldSpecified;
			}
			set
			{
				this.lastSeenFieldSpecified = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("logs", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] logs
		{
			get
			{
				return this.logsField;
			}
			set
			{
				this.logsField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string name
		{
			get
			{
				return this.nameField;
			}
			set
			{
				this.nameField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("OAuthAuthorizationCodes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] OAuthAuthorizationCodes
		{
			get
			{
				return this.oAuthAuthorizationCodesField;
			}
			set
			{
				this.oAuthAuthorizationCodesField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("OAuthIssuedAuthorizationCodes", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] OAuthIssuedAuthorizationCodes
		{
			get
			{
				return this.oAuthIssuedAuthorizationCodesField;
			}
			set
			{
				this.oAuthIssuedAuthorizationCodesField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public long oid
		{
			get
			{
				return this.oidField;
			}
			set
			{
				this.oidField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
		public byte[] passwordHash
		{
			get
			{
				return this.passwordHashField;
			}
			set
			{
				this.passwordHashField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
		public byte[] passwordSalt
		{
			get
			{
				return this.passwordSaltField;
			}
			set
			{
				this.passwordSaltField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("revisions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] revisions
		{
			get
			{
				return this.revisionsField;
			}
			set
			{
				this.revisionsField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public int rid
		{
			get
			{
				return this.ridField;
			}
			set
			{
				this.ridField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("schemas", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] schemas
		{
			get
			{
				return this.schemasField;
			}
			set
			{
				this.schemasField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute("services", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = true)]
		public System.Nullable<long>[] services
		{
			get
			{
				return this.servicesField;
			}
			set
			{
				this.servicesField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public sObjectState state
		{
			get
			{
				return this.stateField;
			}
			set
			{
				this.stateField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool stateSpecified
		{
			get
			{
				return this.stateFieldSpecified;
			}
			set
			{
				this.stateFieldSpecified = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string token
		{
			get
			{
				return this.tokenField;
			}
			set
			{
				this.tokenField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public long userSettingsId
		{
			get
			{
				return this.userSettingsIdField;
			}
			set
			{
				this.userSettingsIdField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public sUserType userType
		{
			get
			{
				return this.userTypeField;
			}
			set
			{
				this.userTypeField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool userTypeSpecified
		{
			get
			{
				return this.userTypeFieldSpecified;
			}
			set
			{
				this.userTypeFieldSpecified = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public string username
		{
			get
			{
				return this.usernameField;
			}
			set
			{
				this.usernameField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
		public byte[] validationToken
		{
			get
			{
				return this.validationTokenField;
			}
			set
			{
				this.validationTokenField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
		public System.DateTime validationTokenCreated
		{
			get
			{
				return this.validationTokenCreatedField;
			}
			set
			{
				this.validationTokenCreatedField = value;
			}
		}

		/// <opmerkingen/>
		[System.Xml.Serialization.XmlIgnoreAttribute()]
		public bool validationTokenCreatedSpecified
		{
			get
			{
				return this.validationTokenCreatedFieldSpecified;
			}
			set
			{
				this.validationTokenCreatedFieldSpecified = value;
			}
		}
	}
}