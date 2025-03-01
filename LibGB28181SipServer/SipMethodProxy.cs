using System;
using System.Threading;
using LibCommon;
using LibCommon.Enums;
using LibCommon.Structs;
using LibCommon.Structs.GB28181;
using LibCommon.Structs.GB28181.XML;
using LibLogger;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LibGB28181SipServer
{
    /// <summary>
    /// sip方法操作代理
    /// </summary>
    public class SipMethodProxy : IDisposable
    {
        private AutoResetEvent _autoResetEvent = new AutoResetEvent(false);
        private SipServer _sipServer;
        private int _timeout;
        private CommandType _commandType;

        public SipMethodProxy(int timeout = 5000)
        {
            _timeout = timeout;
        }

        ~SipMethodProxy()
        {
            Dispose(); //释放非托管资源
        }

        public void Dispose()
        {
            if (_autoResetEvent != null)
            {
                _autoResetEvent.Dispose();
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public CommandType CommandType
        {
            get => _commandType;
            set => _commandType = value;
        }


        /// <summary>
        /// 获取设备的状态信息
        /// </summary>
        /// <param name="sipDevice"></param>
        /// <returns></returns>
        public bool GetSipDeviceStatus(SipDevice sipDevice, out ResponseStruct rs)
        {
            try
            {
                _commandType = CommandType.DeviceStatus;
                Common.SipServer.GetDeviceStatus(sipDevice, _autoResetEvent, out rs, _timeout);
                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理获取设备状态信息失败->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                return true;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// 获取SIP设备信息
        /// </summary>
        /// <param name="sipDevice"></param>
        /// <returns></returns>
        public bool GetSipDeviceInfo(SipDevice sipDevice, out ResponseStruct rs)
        {
            try
            {
                _commandType = CommandType.DeviceInfo;
                Common.SipServer.GetDeviceInfo(sipDevice, _autoResetEvent, out rs, _timeout);
                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理获取设备信息失败->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                return true;
            }
            finally
            {
                Dispose();
            }
        }


        /// <summary>
        /// 获取通道录像文件列表
        /// </summary>
        /// <param name="sipChannel"></param>
        /// <param name="queryRecordFile"></param>
        /// <returns></returns>
        public bool QueryRecordFileList(SipChannel sipChannel, SipQueryRecordFile queryRecordFile,
            out ResponseStruct rs)
        {
            AutoResetEvent _autoResetEvent2 = null;
            try
            {
                _commandType = CommandType.RecordInfo;
                _autoResetEvent2 = new AutoResetEvent(false);
                Common.SipServer.GetRecordFileList(sipChannel, queryRecordFile, _autoResetEvent, _autoResetEvent2,
                    out rs, _timeout);
                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理获取历史视频列表失败->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                isTimeout = _autoResetEvent2.WaitOne(_timeout);
                if (isTimeout)
                {
                    return true;
                }

                Logger.Warn($"[{Common.LoggerHead}]->Sip代理获取历史视频列表失败->{JsonHelper.ToJson(rs)}");
                return false;
            }
            finally
            {
                _autoResetEvent2.Dispose();
                Dispose();
            }
        }

        /// <summary>
        /// ptz控制
        /// </summary>
        /// <param name="ptzCtrl"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public bool PtzMove(PtzCtrl ptzCtrl, out ResponseStruct rs)
        {
            try
            {
                Common.SipServer.PtzMove(ptzCtrl, _autoResetEvent, out rs, _timeout);
                _commandType = CommandType.DeviceControl;
                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理PTZ控制失败->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                return true;
            }
            finally
            {
                Dispose();
            }
        }


        /// <summary>
        /// 请求终止回放流
        /// </summary>
        /// <param name="record"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public bool DeInvite(RecordInfo.RecItem record, out ResponseStruct rs)
        {
            try
            {
                Common.SipServer.DeInvite(record, _autoResetEvent, out rs, _timeout);
                _commandType = CommandType.Unknown;
                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理结束推流失败(历史视频)->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                record.PushStatus = PushStatus.IDLE;
                record.MediaServerStreamInfo = null;
                return true;
            }
            finally
            {
                Dispose();
            }
        }


        /// <summary>
        /// 请求终止时实流
        /// </summary>
        /// <param name="sipChannel"></param>
        /// <returns></returns>
        public bool DeInvite(SipChannel sipChannel, out ResponseStruct rs)
        {
            try
            {
                Common.SipServer.DeInvite(sipChannel, _autoResetEvent, out rs, _timeout);
                _commandType = CommandType.Unknown;

                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理结束推流失败->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                sipChannel.PushStatus = PushStatus.IDLE;

                return true;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// 请求实时流
        /// </summary>
        /// <param name="sipChannel"></param>
        /// <param name="pushMediaInfo"></param>
        /// <returns></returns>
        public bool Invite(SipChannel sipChannel, PushMediaInfo pushMediaInfo, out ResponseStruct rs)
        {
            try
            {
                _commandType = CommandType.Play;
                Common.SipServer.Invite(sipChannel, pushMediaInfo, _autoResetEvent, out rs, _timeout);

                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理推流失败->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                sipChannel.PushStatus = PushStatus.PUSHON;
                return true;
            }
            catch (Exception ex)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sip_InviteExcept,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sip_InviteExcept],
                    ExceptMessage = ex.Message,
                    ExceptStackTrace = ex.StackTrace,
                };
                Logger.Error($"[{Common.LoggerHead}]->Sip代理推流异常->{JsonHelper.ToJson(rs)}");
                return false;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// 请求录像回放流
        /// </summary>
        /// <param name="sipChannel"></param>
        /// <param name="pushMediaInfo"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public bool InviteRecord(RecordInfo.RecItem record, PushMediaInfo pushMediaInfo, out ResponseStruct rs)
        {
            try
            {
                _commandType = CommandType.Playback;
                Common.SipServer.InviteRecord(record, pushMediaInfo, _autoResetEvent, out rs, _timeout);
                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理推流失败(历史视频)->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                record.PushStatus = PushStatus.PUSHON;
                return true;
            }
            catch (Exception ex)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sip_InviteExcept,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sip_InviteExcept],
                    ExceptMessage = ex.Message,
                    ExceptStackTrace = ex.StackTrace,
                };
                Logger.Error($"[{Common.LoggerHead}]->Sip代理推流异常(历史视频)->{JsonHelper.ToJson(rs)}");
                return false;
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// 获取设备目录
        /// </summary>
        /// <param name="sipDevice"></param>
        /// <returns></returns>
        public bool DeviceCatalogQuery(SipDevice sipDevice, out ResponseStruct rs)
        {
            AutoResetEvent _autoResetEvent2 = null;
            try
            {
                _commandType = CommandType.Catalog;
                _autoResetEvent2 = new AutoResetEvent(false);
                Common.SipServer.DeviceCatalogQuery(sipDevice, _autoResetEvent, _autoResetEvent2, out rs, _timeout);
                var isTimeout = _autoResetEvent.WaitOne(_timeout);
                if (!isTimeout || !rs.Code.Equals(ErrorNumber.None))
                {
                    Logger.Warn($"[{Common.LoggerHead}]->Sip代理获取设备目录失败->{JsonHelper.ToJson(rs)}");
                    return false;
                }

                isTimeout = _autoResetEvent2.WaitOne(_timeout);
                if (isTimeout)
                {
                    return true;
                }

                Logger.Warn($"[{Common.LoggerHead}]->Sip代理获取设备目录失败->{JsonHelper.ToJson(rs)}");
                return false;
            }
            finally
            {
                _autoResetEvent2.Dispose();
                Dispose();
            }
        }
    }
}