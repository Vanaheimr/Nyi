/*
 * Copyright (c) 2011-2014, Achim 'ahzf' Friedland <achim@graphdefined.org>
 * This file is part of Vanaheimr Nyi <http://www.github.com/Vanaheimr/Nyi>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using Microsoft.SPOT;

#endregion

namespace eu.Vanaheimr.Nyi.Networking
{

    /// <summary>
    /// An UDP client.
    /// </summary>
    public class UDPClient
    {

        #region Data

        private readonly Socket     InternalSocket;
        private readonly IPAddress  InternalIPAddress;
        private readonly UInt16     InternalPort;
        private const    Int32      Buffersize  = 4096;
        private          Byte[]     Buffer;

        #endregion

        #region Properties

        #region IsBroadcast

        /// <summary>
        /// Returns whether broadcast is enabled.
        /// </summary>
        public Boolean IsBroadcast
        {
            get
            {
                return ((Int32) InternalSocket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast)) == 1;
            }
        }

        #endregion

        #region SocketFlags

        private readonly SocketFlags Socketflags;

        /// <summary>
        /// Returns the socket flags.
        /// </summary>
        public SocketFlags SocketFlags
        {
            get
            {
                return Socketflags;
            }
        }

        #endregion

        #endregion

        #region Constructor(s)

        #region (private) UDPClient(Broadcast, Socketflags)

        private UDPClient(Boolean Broadcast, SocketFlags Socketflags)
        {

            this.Buffer          = new Byte[Buffersize];
            this.Socketflags     = Socketflags;
            this.InternalSocket  = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            if (Broadcast)
                InternalSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

        }

        #endregion

        #region UDPClient(IPEndPoint, Broadcast = false, Socketflags = SocketFlags.None)

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPClient"/> class.
        /// </summary>
        /// <param name="IPEndPoint">The ep.</param>
        public UDPClient(IPEndPoint IPEndPoint, Boolean Broadcast = false, SocketFlags Socketflags = SocketFlags.None)
            : this(Broadcast, Socketflags)
        {
            InternalSocket.Connect(IPEndPoint);
        }

        #endregion

        #region UDPClient(IPAddress, Port, Broadcast = false, Socketflags = SocketFlags.None)

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPClient"/> class.
        /// </summary>
        /// <param name="ep">The ep.</param>
        public UDPClient(IPAddress IPAddress, UInt16 Port, Boolean Broadcast = false, SocketFlags Socketflags = SocketFlags.None)
            : this(Broadcast, Socketflags)
        {

            this.InternalIPAddress  = IPAddress;
            this.InternalPort       = Port;

            InternalSocket.Connect(new IPEndPoint(IPAddress, Port));

        }

        #endregion

        #region UDPClient(Hostname, Port, Broadcast = false, Socketflags = SocketFlags.None)

        /// <summary>
        /// Initializes a new instance of the <see cref="UDPClient"/> class.
        /// </summary>
        /// <param name="Hostname">The hostname.</param>
        /// <param name="Port">The port.</param>
        public UDPClient(String Hostname, UInt16 Port, Boolean Broadcast = false, SocketFlags Socketflags = SocketFlags.None)
            : this(Broadcast, Socketflags)
        {

            if (Hostname == null || Hostname == string.Empty)
                throw new ArgumentNullException("Hostname");

            Socket    tmpSocket      = null;
            Exception lastException  = null;

            try
            {

                var hosts = Dns.GetHostEntry(Hostname);

                for (int i = 0; i < hosts.AddressList.Length; i++)
                {
                    IPAddress iPAddress = hosts.AddressList[i];
                    try
                    {

                        if (InternalSocket == null)
                        {
                            tmpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                            tmpSocket.Connect(new IPEndPoint(iPAddress, Port));
                            InternalSocket = tmpSocket;
                            break;
                        }
                    }
                    catch (SocketException se)
                    {
                        if (se.ErrorCode == 10024) //TooManyOpenFiles/TooManyOpenSockets
                        {
                            var freed = Debug.GC(true);
                            Debug.Print("GC returns " + freed + "bytes");
                        }
                        throw;

                    }
                    catch (Exception ex)
                    {
                        if (ex is ThreadAbortException || ex is OutOfMemoryException)
                        {
                            throw;
                        }
                        lastException = ex;
                    }
                }
            }
            catch (Exception ex)
            {
                lastException = ex;
            }
            finally
            {

                if (InternalSocket != null)
                    InternalSocket.Close();

                if (tmpSocket != null)
                    tmpSocket.Close();

                if (lastException != null)
                    throw lastException;

            }

        }

        #endregion

        #endregion



        /// <summary>
        /// Closes and dispose this instance.
        /// </summary>
        public void Close()
        {
            ((IDisposable) this).Dispose();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public virtual void Dispose()
        {

            Socket socket = InternalSocket;

            if (InternalSocket != null)
                socket.Close();

        }


        ///// <summary>
        ///// Establishes a default remote host using the specified network endpoint.
        ///// </summary>
        ///// <param name="ep">The ep.</param>
        ///// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
        /////   
        ///// <exception cref="T:System.ArgumentNullException">
        /////   <paramref name="endPoint"/> is null. </exception>
        /////   
        ///// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> is closed. </exception>
        //public void Connect(IPEndPoint ep)
        //{
        //    if (ep == null)
        //        throw new ArgumentNullException("endPoint");
        //    this.CheckForBroadcast();
        //    this._socket.Connect(ep);
        //    Active = true;
        //}

        ///// <summary>
        ///// Sends a UDP datagram to the host at the specified remote endpoint.
        ///// </summary>
        ///// <param name="dgram">An array of type <see cref="T:System.Byte"/> that specifies the UDP datagram that you intend to send, represented as an array of bytes.</param>
        ///// <param name="bytes">The number of bytes in the datagram.</param>
        ///// <param name="endPoint">An <see cref="T:System.Net.IPEndPoint"/> that represents the host and port to which to send the datagram.</param>
        ///// <returns>
        ///// The number of bytes sent.
        ///// </returns>
        ///// <exception cref="T:System.ArgumentNullException">
        /////   <paramref name="dgram"/> is null. </exception>
        /////   
        ///// <exception cref="T:System.InvalidOperationException">
        /////   <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> has already established a default remote host. </exception>
        /////   
        ///// <exception cref="T:System.ObjectDisposedException">
        /////   <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> is closed. </exception>
        /////   
        ///// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
        //public int Send(byte[] dgram, int bytes, IPEndPoint endPoint)
        //{
        //    if (dgram == null)
        //        throw new ArgumentNullException("dgram");
        //    if (Active && endPoint != null)
        //        throw new InvalidOperationException("Not connected");
        //    if (endPoint == null)
        //        return this._socket.Send(dgram, 0, bytes, SocketFlags.None);
        //    this.CheckForBroadcast();
        //    return this._socket.SendTo(dgram, 0, bytes, SocketFlags.None, endPoint);
        //}


        ///// <summary>
        ///// Sends a UDP datagram to a specified port on a specified remote host.
        ///// </summary>
        ///// <param name="dgram">An array of type <see cref="T:System.Byte"/> that specifies the UDP datagram that you intend to send represented as an array of bytes.</param>
        ///// <param name="bytes">The number of bytes in the datagram.</param>
        ///// <param name="hostname">The name of the remote host to which you intend to send the datagram.</param>
        ///// <param name="port">The remote port number with which you intend to communicate.</param>
        ///// <returns>
        ///// The number of bytes sent.
        ///// </returns>
        ///// <exception cref="T:System.ArgumentNullException">
        /////   <paramref name="dgram"/> is null. </exception>
        /////   
        ///// <exception cref="T:System.InvalidOperationException">The <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> has already established a default remote host. </exception>
        /////   
        ///// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> is closed. </exception>
        /////   
        ///// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
        //public int Send(byte[] dgram, int bytes, string hostname, int port)
        //{

        //    if (dgram == null)
        //        throw new ArgumentNullException("dgram");
        //    if (Active && (hostname != null || port != 0))
        //        throw new InvalidOperationException("Not connected");
        //    if (hostname == null || port == 0)
        //        return this._socket.Send(dgram, 0, bytes, SocketFlags.None);

        //    var hosts = Dns.GetHostEntry(hostname);
        //    int num = 0;

        //    if (hosts.AddressList.Length == 0 || num == hosts.AddressList.Length)
        //    {
        //        throw new ArgumentException("Invalid List", "hostname");
        //    }

        //    this.CheckForBroadcast();

        //    IPEndPoint remoteEP = new IPEndPoint(hosts.AddressList[num], port);
        //    return this._socket.SendTo(dgram, 0, bytes, SocketFlags.None, remoteEP);

        //}



        //static byte[] GetBytes(string str)
        //{
        //    byte[] bytes = new byte[str.Length * sizeof(char)];
        //    System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        //    return bytes;
        //}

        //static string GetString(byte[] bytes)
        //{
        //    char[] chars = new char[bytes.Length / sizeof(char)];
        //    System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        //    return new string(chars);
        //}


        public Int32 Send(String Data, UInt32 Offset = 0, UInt32 NumberOfBytes = 0)
        {
            return Send(System.Text.Encoding.UTF8.GetBytes(Data), Offset, NumberOfBytes);
        }


        /// <summary>
        /// Sends a UDP datagram to a remote host.
        /// </summary>
        /// <param name="Data">An array of type <see cref="T:System.Byte"/> that specifies the UDP datagram that you intend to send represented as an array of bytes.</param>
        /// <param name="bytes">The number of bytes in the datagram.</param>
        /// <returns>
        /// The number of bytes sent.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="Data"/> is null. </exception>
        ///   
        /// <exception cref="T:System.InvalidOperationException">The <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> has already established a default remote host. </exception>
        ///   
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> is closed. </exception>
        ///   
        /// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
        public Int32 Send(Byte[] Data, UInt32 Offset = 0, UInt32 NumberOfBytes = 0)
        {

            if (Data == null)
                throw new ArgumentNullException("dgram");

            if (InternalSocket != null)
            {

                UInt16 Retries = 5;

                do
                {

                    try
                    {

                        return this.InternalSocket.Send(Data,
                                                        (Int32) Offset,
                                                        (NumberOfBytes > 0) ? (Int32) NumberOfBytes : (Int32) (Data.Length - Offset),
                                                        Socketflags);

                    }
                    catch (Exception e1)
                    {

                        Debug.Print("UDP send error: " + e1.Message);

                        try
                        {
                            InternalSocket.Connect(new IPEndPoint(InternalIPAddress, InternalPort));
                        }
                        catch (Exception e2)
                        {
                            Debug.Print("UDP socket reset error: " + e2.Message);
                        }

                        Retries--;

                    }

                } while (Retries > 0);

            }

            return 0;

        }

        /// <summary>
        /// Returns a UDP datagram that was sent by a remote host.
        /// </summary>
        /// <param name="remoteEP">An <see cref="T:System.Net.IPEndPoint"/> that represents the remote host from which the data was sent.</param>
        /// <returns>
        /// An array of type <see cref="T:System.Byte"/> that contains datagram data.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">The underlying <see cref="T:System.Net.Sockets.Socket"/> has been closed. </exception>
        ///   
        /// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
        public Byte[] Receive(ref IPEndPoint remoteEP)
        {

            EndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
            var num = this.InternalSocket.ReceiveFrom(Buffer, Buffersize, SocketFlags.None, ref endPoint);
            remoteEP = (IPEndPoint)endPoint;

            if (num < 65536)
            {
                var array = new Byte[num];
                Array.Copy(this.Buffer, 0, array, 0, num);
                return array;
            }

            return Buffer;

        }

        /*
        /// <summary>
        /// Adds a <see cref="T:Ranzmaier.NETMF.GadgeteerNetworking.UDPClient"/> to a multicast group.
        /// </summary>
        /// <param name="multicastAddr">The multicast <see cref="T:System.Net.IPAddress"/> of the group you want to join.</param>
        /// <exception cref="T:System.ObjectDisposedException">The underlying <see cref="T:System.Net.Sockets.Socket"/> has been closed. </exception>
        ///   
        /// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
        public void JoinMulticastGroup(IPAddress multicastAddr)
        {

            if (multicastAddr == null)
                throw new ArgumentNullException("multicastAddr");

            this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, multicastAddr.GetAddressBytes());
            //this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastAddr.GetAddressBytes());
        }

        /// <summary>
        /// Leaves a multicast group.
        /// </summary>
        /// <param name="multicastAddr">The <see cref="T:System.Net.IPAddress"/> of the multicast group to leave.</param>
        /// <exception cref="T:System.ObjectDisposedException">The underlying <see cref="T:System.Net.Sockets.Socket"/> has been closed. </exception>
        ///   
        /// <exception cref="T:System.Net.Sockets.SocketException">An error occurred when accessing the socket. See the Remarks section for more information. </exception>
        ///   
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="multicastAddr"/> is null.</exception>
       
        public void DropMulticastGroup(IPAddress multicastAddr)
        {
            if (multicastAddr == null)
                throw new ArgumentNullException("multicastAddr");
            this.Client.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.DropMembership, multicastAddr.GetAddressBytes());
        }
         * 
         */

    }

}
