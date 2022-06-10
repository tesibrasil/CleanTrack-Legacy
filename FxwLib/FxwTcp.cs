using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace It.IDnova.Fxw
{
	internal class FxwTcp : IFxwPhysicalLayer
	{
		private FxwLog _logger = FxwLog.getInstance();
		private Socket _theSocket = (Socket)null;
		private byte[] _receiveBuf = (byte[])null;
		private bool _useAsciiTransport = false;
		private const int RECEIVE_BUF_SIZE = 2048;
		private bool _useExternalTransport;

		public FxwTcp(PhysicalReceiveDataHandler callback)
		  : base(callback)
		{
			this._theSocket = (Socket)null;
			this._useAsciiTransport = false;
			this._receiveBuf = new byte[2048];
		}

		public override bool isValid()
		{
			if (this._theSocket == null)
				return false;
			return this._theSocket.Connected;
		}

		public override bool isConnected()
		{
			bool flag = false;
			if (this._theSocket == null)
				return flag;
			bool blocking = this._theSocket.Blocking;
			try
			{
				this._theSocket.Blocking = false;
				this._theSocket.Send(new byte[1], 0, SocketFlags.None);
				flag = true;
			}
			catch (SocketException ex)
			{
				if (ex.NativeErrorCode.Equals(10035))
					flag = true;
				else
					this._logger.debug("[FxwTcp] Disconnected: err[0]" + (object)ex.NativeErrorCode);
			}
			finally
			{
				this._theSocket.Blocking = blocking;
			}
			return flag;
		}

		public override byte connect(string aConnString, bool useAsciiTransport, object transportChannel, int txDelayMs)
		{
			this._useAsciiTransport = useAsciiTransport;
			if (transportChannel != null)
			{
				this._useExternalTransport = true;
				this._theSocket = (Socket)transportChannel;
			}
			if (aConnString == null || aConnString.Equals(""))
				return byte.MaxValue;
			string[] strArray = aConnString.Split(':');
			if (strArray.Length != 2)
				return 2;
			try
			{
				string ipString = strArray[0];
				int port = int.Parse(strArray[1]);
				if (!this._useExternalTransport)
				{
					this._theSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					this._theSocket.Connect((EndPoint)new IPEndPoint(IPAddress.Parse(ipString), port));
				}
				byte[] buffer = new byte[2048];
				int num1 = 10;
				int num2 = 0;
				int num3 = 0;
				this._logger.debug("[FxwTcp] Receiving useless input buffer...");
				while (num1 > 0)
				{
					try
					{
						if (!this._useExternalTransport)
						{
							this._theSocket.ReceiveTimeout = 1000;
							num2 = this._theSocket.Receive(buffer, SocketFlags.None);
						}
					}
					catch
					{
						if (!this._theSocket.Connected && !this._useExternalTransport)
						{
							this._theSocket.Close();
							this._theSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
							this._theSocket.Connect((EndPoint)new IPEndPoint(IPAddress.Parse(ipString), port));
							break;
						}
						break;
					}
					num3 += num2;
					--num1;
					if (num2 < 512)
						break;
				}
				this._logger.debug("[FxwTcp] Discarded data: " + (object)num3 + " bytes");
				this._theSocket.ReceiveTimeout = 0;
				this._theSocket.BeginReceive(this._receiveBuf, 0, 2048, SocketFlags.None, new AsyncCallback(this.onReceivedData), (object)this);
				this._logger.debug("[FxwTcp] Connection established");
			}
			catch (Exception ex)
			{
				this._logger.debug("[FxwTcp] Connection exception: " + ex.Message);
				if (this._theSocket != null && !this._useExternalTransport)
				{
					this._theSocket.Disconnect(false);
					this._theSocket.Close();
					this._logger.debug("[FxwTcp] Socket disconnected and closed");
					this._theSocket = (Socket)null;
				}
				return 3;
			}
			return 0;
		}

		public override void disconnect()
		{
			if (!this._useExternalTransport)
			{
				try
				{
					if (this.isValid() && this._theSocket != null)
					{
						this._logger.debug("[FxwTcp] Terminating TCP/IP connection...");
						this._logger.debug("[FxwTcp] Closing...");
						this._theSocket.Close();
					}
					this._logger.debug("[FxwTcp] Socket disconnected");
				}
				catch (Exception)
				{
					this._logger.debug("[FxwTcp] Connection termination exception");
				}
			}
			this._theSocket = (Socket)null;
		}

		public override byte sendData(byte[] dataBuff, int dataLen)
		{
			if (!this.isValid())
				return 1;

			byte num2 = 0;
			try
			{
				int num3;
				if (this._useAsciiTransport)
				{
					num3 = this._theSocket.Send(Encoding.ASCII.GetBytes(RfidUtils.byteArrayToHexString(dataBuff)), SocketFlags.None);
					if (num3 > 0)
						num3 /= 2;
				}
				else
				{
					num3 = this._theSocket.Send(dataBuff, SocketFlags.None);
				}

				this._logger.debug("[FxwTcp] Msg sent over TCP (" + (object)num3 + " bytes)");
			}
			catch (SocketException)
			{
				this._logger.debug("[FxwTcp] Socket error sending data");
				num2 = 4;

				// Sandro 10/05/2017 // devo gestire l'interruzione della connessione (qui e/o nel onReceivedData) //
			}
			return num2;
		}

		private void onReceivedData(IAsyncResult asynchRes)
		{
			try
			{
				if (this._theSocket == null)
					return;
				int num1 = this._theSocket.EndReceive(asynchRes);
				if (num1 > 0)
				{
					int dataLen = 0;
					byte[] dataBuff = (byte[])null;
					if (this._useAsciiTransport)
					{
						if ((uint)(num1 % 2) <= 0U)
						{
							try
							{
								string s = "";
								for (int index = 0; index < num1; ++index)
									s += ((char)this._receiveBuf[index]).ToString();
								dataBuff = RfidUtils.hexStringToByteArray(s);
								dataLen = dataBuff.Length;
							}
							catch
							{
								dataBuff = (byte[])null;
							}
						}
					}
					else
					{
						dataLen = num1;
						dataBuff = new byte[dataLen];
						for (int index = 0; index < dataLen; ++index)
							dataBuff[index] = this._receiveBuf[index];
						string str = "";
						foreach (byte num2 in dataBuff)
							str = str + num2.ToString("X2") + " ";
						this._logger.debug("[FxwTcp] Received " + (object)dataLen + " bytes: " + str);
					}
					if (dataBuff != null)
						this.receivedDataCallback(dataBuff, dataLen);
				}
				else if (num1 <= 0)
				{
					this.receivedDataCallback(new byte[0], 0);
					this._theSocket.Close();
					this._theSocket = (Socket)null;
				}
				if (this._theSocket == null)
					return;
				this._theSocket.BeginReceive(this._receiveBuf, 0, 2048, SocketFlags.None, new AsyncCallback(this.onReceivedData), (object)this);
			}
			catch (SocketException)
			{
				this._logger.debug("[FxwTcp] Socket error receiving data");

				// Sandro 10/05/2017 // devo gestire l'interruzione della connessione (qui e/o nel sendData) //

				this.receivedDataCallback(new byte[0], 0);
				this._theSocket.Close();
				this._theSocket = (Socket)null;
			}
			catch (ObjectDisposedException)
			{
				this._logger.debug("[FxwTcp] Data receiving interrupted (socket closed)");
			}
			catch (Exception)
			{
				this._logger.debug("[FxwTcp] Socket error receiving data");
			}
		}
	}
}
