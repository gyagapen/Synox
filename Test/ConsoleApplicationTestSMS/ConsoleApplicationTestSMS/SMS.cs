using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApplicationTestSMS
{
	public class SMS : SMSBase
	{
		#region Members
		protected bool _moreMessagesToSend;
		protected bool _rejectDuplicates;
		protected byte _messageReference;
		protected string _phoneNumber;
		protected byte _protocolIdentifier;
		protected byte _dataCodingScheme;
		protected byte _validityPeriod;
		protected DateTime _serviceCenterTimeStamp;
		protected string _userData;
		protected byte[] _userDataHeader;
		protected string _message;
		#endregion

		#region Properties
		public DateTime ServiceCenterTimeStamp { get { return _serviceCenterTimeStamp; } }

		public byte MessageReference
		{
			get { return _messageReference; }
			set { _messageReference = value; }
		}

		public string PhoneNumber
		{
			get { return _phoneNumber; }
			set { _phoneNumber = value; }
		}

        /// <summary>
        /// Gets or Sets the message body to be sent,
        /// updated by [www.codeproblem.com]
        /// </summary>
		public string Message
		{
			get { return _message; }
			set
			{
				_message = value;
			}
		}

		public bool RejectDuplicates
		{
			get
			{
				if (Direction == SMSDirection.Received)
					throw new InvalidOperationException("Received message can not contains 'reject duplicates' property");

				return _rejectDuplicates;
			}
			set
			{
				if (Direction == SMSDirection.Received)
					throw new InvalidOperationException("Received message can not contains 'reject duplicates' property");

				_rejectDuplicates = value;
			}
		}

		public bool MoreMessagesToSend
		{
			get
			{
				if (Direction == SMSDirection.Received)
					throw new InvalidOperationException("Submited message can not contains 'more message to send' property");
				
				return _moreMessagesToSend;
			}
			set
			{
				if (Direction == SMSDirection.Received)
					throw new InvalidOperationException("Submited message can not contains 'more message to send' property");

				_moreMessagesToSend = value;
			}
		}

		public TimeSpan ValidityPeriod
		{
			get
			{
				if (_validityPeriod > 196)
					return new TimeSpan((_validityPeriod - 192) * 7, 0, 0, 0);

				if (_validityPeriod > 167)
					return new TimeSpan((_validityPeriod - 166), 0, 0, 0);

				if (_validityPeriod > 143)
					return new TimeSpan(12, (_validityPeriod - 143) * 30, 0);

				return new TimeSpan(0, (_validityPeriod + 1) * 5, 0);
			}
			set
			{
				if (value.Days > 441)
					throw new ArgumentOutOfRangeException("TimeSpan.Days", value.Days, "Value must be not greater 441 days.");

				if (value.Days > 30) //Up to 441 days
					_validityPeriod = (byte) (192 + (int) (value.Days / 7));
				else if (value.Days > 1) //Up to 30 days
					_validityPeriod = (byte) (166 + value.Days);
				else if (value.Hours > 12) //Up to 24 hours
					_validityPeriod = (byte) (143 + (value.Hours - 12) * 2 + value.Minutes / 30);
				else if (value.Hours > 1 || value.Minutes > 1) //Up to 12 hours
					_validityPeriod = (byte) (value.Hours * 12 + value.Minutes / 5 - 1);
				else {
					_validityPeriodFormat = ValidityPeriodFormat.FieldNotPresent;
					
					return;
				}

				_validityPeriodFormat = ValidityPeriodFormat.Relative;
			}
		}

		public virtual byte[] UserDataHeader { get { return _userDataHeader; } }

		#region "in parts" message properties
        /// <summary>
        /// Gets or Sets the InParts property of this SMS,
        /// Set it to true for Multipart SMS
        /// [www.codeproblem.com]
        /// </summary>
		public bool InParts
		{
			get
			{
				if (_userDataHeader == null || _userDataHeader.Length < 5)
					return false;

				return (_userDataHeader[0] == 0x00 && _userDataHeader[1] == 0x03); // | 08 04 00 | 9F 02 | i have this header from siemenes in "in parts" message
			}
            //Updated code block start www.codeproblem.com - October 18, 2009
            set
            {
                if (value == true)
                {
                    if (Direction == SMSDirection.Received)
                        throw new InvalidOperationException("InParts propert is read only for received message");
                    else
                    {
                        _userDataHeader = new byte[5];
                        _userDataHeader[0] = 0x00;
                        _userDataHeader[1] = 0x03; //Multipart SMS
                    }
                }
            }
            //Updated code block end www.codeproblem.com - October 18, 2009
		}

        /// <summary>
        /// Gets or Sets the Reference ID of SMS in this Long SMS
        /// [www.codeproblem.com]
        /// </summary>
		public int InPartsID
		{
			get
			{
				if (!InParts)
					return 0;

				//return (_userDataHeader[2] << 8) + _userDataHeader[3];
                return (_userDataHeader[2]); //Reference ID
			}
            //Updated code block start www.codeproblem.com - October 18, 2009
            set
            {
                if (value > 255)
                {
                    throw new ArgumentOutOfRangeException("Message Reference ID can be between 0-255");
                }
                if (Direction == SMSDirection.Received)
                {
                    throw new InvalidOperationException("InPartsID is read only for received message");
                }
                if (!InParts)
                {
                    throw new InvalidOperationException("First set InParts before setting InPartsID");
                }
                
                //assigning new value for Message Reference to _userDataHeader
                _userDataHeader[2] = (byte)value;

            }
            //Updated code block end www.codeproblem.com - October 18, 2009
		}

        /// <summary>
        /// Gets or Sets the total number of messages in this long SMS
        /// [www.codeproblem.com]
        /// </summary>
        public int TotalParts
        { 
            get 
            { 
                if (!InParts)
                    return 0;
                return _userDataHeader[3]; 
            }
            //Updated code block start www.codeproblem.com - October 18, 2009
            set
            {
                if (value > 255)
                {
                    throw new ArgumentOutOfRangeException("Total number of parts can't be more than 255");
                }
                if (Direction == SMSDirection.Received)
                {
                    throw new InvalidOperationException("TotalParts is read only for received message");
                }
                if (!InParts)
                {
                    throw new InvalidOperationException("First set InParts before setting TotalParts");
                }

                //setting the new value for Total Messages
                _userDataHeader[3] = (byte)value;
            }
            //Updated code block end www.codeproblem.com - October 18, 2009
        }
        
        /// <summary>
        /// Gets or Sets this part number for long SMS
        /// [www.codeproblem.com]
        /// </summary>
		public int Part
		{
			get
			{
				if (!InParts)
					return 0;

				return _userDataHeader[4];
			}
            //Updated code block start www.codeproblem.com - October 18, 2009
            set
            {
                if (value > 255)
                {
                    throw new ArgumentOutOfRangeException("Part can't be more than 255");
                }
                if (Direction == SMSDirection.Received)
                {
                    throw new InvalidOperationException("Part is read only for received message");
                }
                if (!InParts)
                {
                    throw new InvalidOperationException("First set InParts before setting Part");
                }

                //Set the new Part value in _userdataHeader
                _userDataHeader[4] = (byte)value;
            }
            //Updated code block end www.codeproblem.com - October 18, 2009
		}
		#endregion

		public override SMSType Type { get { return SMSType.SMS; } }
		#endregion

		#region Public Statics
		public static void Fetch(SMS sms, ref string source)
		{
			SMSBase.Fetch(sms, ref source);

			if (sms._direction == SMSDirection.Submited)
				sms._messageReference = PopByte(ref source);

			sms._phoneNumber = PopPhoneNumber(ref source);
			sms._protocolIdentifier = PopByte(ref source);
			sms._dataCodingScheme = PopByte(ref source);

            if (sms._direction == SMSDirection.Submited)
            {
                if (sms._validityPeriod != 0x00)
                    sms._validityPeriod = PopByte(ref source);
            }

			if (sms._direction == SMSDirection.Received)
				sms._serviceCenterTimeStamp = PopDate(ref source);

			sms._userData = source;

			if (source == string.Empty)
				return;

			int userDataLength = PopByte(ref source);

			if (userDataLength == 0)
				return;

			if (sms._userDataStartsWithHeader) {
				byte userDataHeaderLength = PopByte(ref source);
				
				sms._userDataHeader = PopBytes(ref source, userDataHeaderLength);

				userDataLength -= userDataHeaderLength + 1;
			}

			if (userDataLength == 0)
				return;

			switch ((SMSEncoding) sms._dataCodingScheme & SMSEncoding.ReservedMask) {
				case SMSEncoding._7bit:
					sms._message = Decode7bit(source, userDataLength);
					break;
				case SMSEncoding._8bit:
					sms._message = Decode8bit(source, userDataLength);
					break;
				case SMSEncoding.UCS2:
					sms._message = DecodeUCS2(source, userDataLength);
					break;
			}
		}
		#endregion

		#region Publics
        
        /// <summary>
        /// Sets the PDU type for this message e.g. Delivery Report, Multipart SMS properties,
        /// updated by [www.codeproblem.com]
        /// </summary>
		public override void ComposePDUType()
		{
			base.ComposePDUType();

			if (_moreMessagesToSend || _rejectDuplicates)
				_pduType = (byte) (_pduType | 0x04);

            //Updated code block start www.codeproblem.com - October 18, 2009
            if(InParts)
                _pduType = (byte) (_pduType | 0x40);
            //Updated code block end www.codeproblem.com - October 18, 2009
		}

        /// <summary>
        /// Compose a SMS using the required encoding, Supports 16 and 7 bit encoding
        /// 7-bit encoding implemented by [www.codeproblem.com]
        /// </summary>
        /// <param name="messageEncoding">SMS encoding type</param>
        /// <returns>string of packed SMS</returns>
		public virtual string Compose(SMSEncoding messageEncoding)
		{
            if (messageEncoding == SMSEncoding.UCS2)
            {
                if (_message.Length > 70)
                    throw new ArgumentOutOfRangeException("Message.Length", _message.Length, "Message length can not be greater that 70 chars for unicode messages.");
            }
            if (messageEncoding == SMSEncoding._7bit)
            {
                if(_message.Length > 160)
                    throw new ArgumentOutOfRangeException("Message.Length", _message.Length, "Message length can not be greater that 160 chars for 7-bit messages.");
            }
			ComposePDUType();

			string encodedData = "00"; //Length of SMSC information. Here the length is 0, which means that the SMSC stored in the phone should be used. Note: This octet is optional. On some phones this octet should be omitted! (Using the SMSC stored in phone is thus implicit)

			encodedData += Convert.ToString(_pduType, 16).PadLeft(2, '0'); //PDU type (forst octet)
			encodedData += Convert.ToString(MessageReference, 16).PadLeft(2, '0');
			encodedData += EncodePhoneNumber(PhoneNumber);
			encodedData += "00"; //Protocol identifier (Short Message Type 0)
			encodedData += Convert.ToString((int) messageEncoding, 16).PadLeft(2, '0'); //Data coding scheme

			if (_validityPeriodFormat != ValidityPeriodFormat.FieldNotPresent)
				encodedData += Convert.ToString(_validityPeriod, 16).PadLeft(2, '0'); //Validity Period


			byte[] messageBytes = null;

			switch (messageEncoding) {
				case SMSEncoding.UCS2:
					messageBytes = EncodeUCS2(_message);
                    encodedData += Convert.ToString(messageBytes.Length, 16).PadLeft(2, '0'); //Length of message
					break;
                case SMSEncoding._7bit:
                    messageBytes = Encode7bit(_message);
                    encodedData += Convert.ToString(_message.Length, 16).PadLeft(2, '0'); //Length of message
                    break;
				default:
					messageBytes = new byte[0];
					break;
			}
                      

			foreach (byte b in messageBytes)
				encodedData += Convert.ToString(b, 16).PadLeft(2, '0');

			return encodedData.ToUpper();
		}


        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// Compose a Multipart SMS and returns the string array of PDU for each SMS part,
        /// Support only 7-bit encoding
        /// [www.codeproblem.com]
        /// </summary>
        /// <param name="messageEncoding">Message Encoding used</param>
        /// <returns>string[] of SMS PDUs</returns>
        public string[] ComposeLongSMS(SMSEncoding messageEncoding)
        {
            //setting the in parts properties
            InParts = true;

            //Generate a random Message ID
            InPartsID = RandomNumber(1, 254);

            //Setting total parts field
            int parts = (int)(Math.Ceiling(((double)(_message.Length) / 153)));
            TotalParts = parts;

            if (!InParts)
            {
                throw new InvalidOperationException("Set the InParts property before calling ComposeLongSMS");
            }
            if (_userDataHeader.Length != 5)
            {
                throw new InvalidOperationException("UserDataHeader field must contain valid values for long SMS");
            }
            
            //storing the origional message for backup
            string origionalMessage = _message;

            //Final SMS strings
            string[] strSMSArray = new string[parts];

            //now break the long SMS in parts
            for (int i = 1; i <= parts; i++)
            {
                Part = i;
                
                //First get 7-bit ASCII for the UDH
                byte[] _udh = new byte[6];
                _udh[0] = 5; //length of header
                Array.Copy(_userDataHeader, 0, _udh, 1, _userDataHeader.Length);
                
                //Now Encode the header
                string header = EncodeMultiPartHeader(_udh);
                
                int lengthToCut = ((((i - 1) * 153) + 153) > origionalMessage.Length) ? origionalMessage.Length - ((i - 1) * 153) : 153;
                _message = origionalMessage.Substring((i - 1) * 153, lengthToCut);
                
                //adding user data header to message
                _message = header + _message; 
                
                //Compose this single sms
                string finalmessage = Compose(messageEncoding);

                //Storing in array
                strSMSArray[i - 1] = finalmessage;
            }

            //returning the array
            return strSMSArray;
        }


        public string EncodeMultiPartHeader(byte[] header)
        {
            byte mask = 0;
            byte shiftedMask = 0;
            byte bitsRequired = 0;
            byte invertMask = 0;
            byte previuosbitsRequired = 0;
            //header will now contain 7 bytes
            byte[] encodedHeader = new byte[7];
            int i = 0;
            for (i = 0; i < header.Length; i++)
            {
                mask = (byte)((mask * 2) + 1);
                shiftedMask = (byte)(mask << 7 - i);
                bitsRequired = (byte)(header[i] & shiftedMask);
                invertMask = (byte)~shiftedMask;
                encodedHeader[i] = (byte)(header[i] & invertMask);
                bitsRequired = (byte)(bitsRequired >> 7 - i);
                encodedHeader[i] = (byte)(encodedHeader[i] << i);
                encodedHeader[i] = (byte)(encodedHeader[i] | previuosbitsRequired);
                previuosbitsRequired = bitsRequired;
            }
            encodedHeader[i] = previuosbitsRequired;

            return Encoding.ASCII.GetString(encodedHeader);
        }

        #endregion

        public enum SMSEncoding
		{
			ReservedMask = 0x0C /*1100*/, 
			_7bit = 0, 
			_8bit = 0x04 /*0100*/, 
			UCS2 = 0x08 /*1000*/
		}
	}
}
