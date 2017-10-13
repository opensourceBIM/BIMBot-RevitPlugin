using System.Xml.Serialization;

namespace BimServerExchange.Runtime
{
	[XmlRoot("LoginResponse", Namespace = "org.bimserver")]
	public class LoginToken
	{
		[XmlElement("return")]
		public string Token { get; set; }
	}

	/// <opmerkingen/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "org.bimserver")]
	public enum sUserType
	{

		/// <opmerkingen/>
		SYSTEM,

		/// <opmerkingen/>
		ADMIN,

		/// <opmerkingen/>
		USER,

		/// <opmerkingen/>
		READ_ONLY,
	}

	/// <opmerkingen/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "org.bimserver")]
	public enum sAccessMethod
	{

		/// <opmerkingen/>
		SOAP,

		/// <opmerkingen/>
		WEB_INTERFACE,

		/// <opmerkingen/>
		INTERNAL,

		/// <opmerkingen/>
		REST,

		/// <opmerkingen/>
		SYNDICATION,

		/// <opmerkingen/>
		JSON,

		/// <opmerkingen/>
		PROTOCOL_BUFFERS,
	}

	/// <opmerkingen/>
	[System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.6.1055.0")]
	[System.SerializableAttribute()]
	[System.Xml.Serialization.XmlTypeAttribute(Namespace = "org.bimserver")]
	public enum sObjectState
	{

		/// <opmerkingen/>
		ACTIVE,

		/// <opmerkingen/>
		DELETED,
	}
}
