using System.Collections.Generic;
using LibCommon;
using LibCommon.Structs.WebRequest.AKStreamKeeper;
using LibCommon.Structs.WebResponse.AKStreamKeeper;
using LibLogger;
using Newtonsoft.Json;

namespace AKStreamWeb.Services
{
    public static class AKStreamKeeperService
    {
        /// <summary>
        /// 添加一个裁剪合并任务
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="reqKeeper"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static ResKeeperCutMergeTaskResponse AddCutOrMergeTask(string mediaServerId,
            ReqKeeperCutMergeTask reqKeeper, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->添加裁剪合并任务失败->{mediaServerId}->{JsonHelper.ToJson(reqKeeper)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");
                return null;
            }

            if (reqKeeper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->添加裁剪合并任务失败->{mediaServerId}->{JsonHelper.ToJson(reqKeeper)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->添加裁剪合并任务失败->{mediaServerId}->{JsonHelper.ToJson(reqKeeper)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->添加裁剪合并任务失败->{mediaServerId}->{JsonHelper.ToJson(reqKeeper)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var ret = mediaServer.KeeperWebApi.AddCutOrMergeTask(out rs, reqKeeper);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->添加裁剪合并任务失败->{mediaServerId}->{JsonHelper.ToJson(reqKeeper)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            Logger.Info(
                $"[{Common.LoggerHead}]->添加裁剪合并任务成功->{mediaServerId}->{JsonHelper.ToJson(reqKeeper)}->{JsonHelper.ToJson(ret)}");
            return ret;
        }

        /// <summary>
        /// 获取裁剪合并任务状态
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="taskId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static ResKeeperCutMergeTaskStatusResponse GetMergeTaskStatus(string mediaServerId, string taskId,
            out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务状态失败->{mediaServerId}->{taskId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (string.IsNullOrEmpty(taskId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务状态失败->{mediaServerId}->{taskId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务状态失败->{mediaServerId}->{taskId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务状态失败->{mediaServerId}->{taskId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var ret = mediaServer.KeeperWebApi.GetMergeTaskStatus(out rs, taskId);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务状态失败->{mediaServerId}->{taskId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            Logger.Info($"[{Common.LoggerHead}]->获取合并裁剪任务状态成功->{mediaServerId}->{taskId}->{JsonHelper.ToJson(ret)}");
            return ret;
        }

        /// <summary>
        /// 获取裁剪合并任务积压情况
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static ResKeeperCutMergeTaskStatusResponseList GetBacklogTaskList(string mediaServerId,
            out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务积压列表失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }


            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务积压列表失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务积压列表失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var ret = mediaServer.KeeperWebApi.GetBacklogTaskList(out rs);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取合并裁剪任务积压列表失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            Logger.Info($"[{Common.LoggerHead}]->获取合并裁剪任务积压列表成功->{mediaServerId}->{JsonHelper.ToJson(ret)}");

            return ret;
        }

        /// <summary>
        /// 获取一个可用的rtp端口（偶数端口）
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ushort GuessAnRtpPort(string mediaServerId, out ResponseStruct rs, ushort? min = 0,
            ushort? max = 0)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取可用的rtp端口失败->{mediaServerId}-portMin:{min}-portMax:{max}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return 0;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取可用的rtp端口失败->{mediaServerId}-portMin:{min}-portMax:{max}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return 0;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取可用的rtp端口失败->{mediaServerId}-portMin:{min}-portMax:{max}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return 0;
            }

            var ret = mediaServer.KeeperWebApi.GuessAnRtpPort(out rs, min, max);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->获取可用的rtp端口失败->{mediaServerId}-portMin:{min}-portMax:{max}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return 0;
            }

            Logger.Info($"[{Common.LoggerHead}]->获取可用的rtp端口成功->{mediaServerId}-portMin:{min}-portMax:{max}->端口:{ret}");

            return ret;
        }

        /// <summary>
        /// 删除一个指定文件
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="filePath"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static bool DeleteFile(string mediaServerId, string filePath, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (UtilsHelper.StringIsNullEx(filePath))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var ret = mediaServer.KeeperWebApi.DeleteFile(out rs, filePath);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            Logger.Info($"[{Common.LoggerHead}]->删除指定文件成功->{mediaServerId}->{filePath}");

            return ret;
        }

        /// <summary>
        /// 获取流媒体治理程序健康状态
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static bool KeeperHealth(string mediaServerId, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->与AKStreamKeeper保持心跳失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }


            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->与AKStreamKeeper保持心跳失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_AKStreamKeeperNotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_AKStreamKeeperNotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->与AKStreamKeeper保持心跳失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var ret = mediaServer.KeeperWebApi.KeeperHealth(out rs);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->与AKStreamKeeper保持心跳失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            Logger.Debug($"[{Common.LoggerHead}]->与AKStreamKeeper保持心跳成功->{mediaServerId}");

            return ret;
        }

        /// <summary>
        /// 指定文件是否存在
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="filePath"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static bool FileExists(string mediaServerId, string filePath, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检查指定文件是否存在失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (UtilsHelper.StringIsNullEx(filePath))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检查指定文件是否存在失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检查指定文件是否存在失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检查指定文件是否存在失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var ret = mediaServer.KeeperWebApi.FileExists(out rs, filePath);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检查指定文件是否存在失败->{mediaServerId}->{filePath}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            Logger.Debug($"[{Common.LoggerHead}]->检查指定文件是否存在成功->{mediaServerId}->{filePath}->{ret}");

            return ret;
        }

        /// <summary>
        /// 删除文件列表
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="fileList"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static ResKeeperDeleteFileList DeleteFileList(string mediaServerId, List<string> fileList,
            out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件列表失败->{mediaServerId}->{JsonHelper.ToJson(fileList, Formatting.Indented)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (fileList == null || fileList.Count <= 0)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件列表失败->{mediaServerId}->{JsonHelper.ToJson(fileList, Formatting.Indented)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件列表失败->{mediaServerId}->{JsonHelper.ToJson(fileList, Formatting.Indented)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件列表失败->{mediaServerId}->{JsonHelper.ToJson(fileList, Formatting.Indented)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var ret = mediaServer.KeeperWebApi.DeleteFileList(out rs, fileList);
            if (ret == null || !rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->删除指定文件列表失败->{mediaServerId}->{JsonHelper.ToJson(fileList, Formatting.Indented)}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            Logger.Info(
                $"[{Common.LoggerHead}]->删除指定文件列表成功->{mediaServerId}->{JsonHelper.ToJson(fileList, Formatting.Indented)}->{JsonHelper.ToJson(ret)}");

            return ret;
        }


        /// <summary>
        /// 清空空目录
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool CleanUpEmptyDir(string mediaServerId, out ResponseStruct rs, string? filePath = "")
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->清理空目录失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->清理空目录失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->清理空目录失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var ret = mediaServer.KeeperWebApi.CleanUpEmptyDir(out rs, filePath);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->清理空目录失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            Logger.Info($"[{Common.LoggerHead}]->清理空目录成功->{mediaServerId}");

            return ret;
        }

        /// <summary>
        /// 启动流媒体服务
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static ResKeeperStartMediaServer StartMediaServer(string mediaServerId, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->启动流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->启动流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_AKStreamKeeperNotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_AKStreamKeeperNotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->启动流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var ret = mediaServer.KeeperWebApi.StartMediaServer(out rs);
            if (ret == null || !rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->启动流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            Logger.Info($"[{Common.LoggerHead}]->启动流媒体服务成功->{mediaServerId}->{JsonHelper.ToJson(ret)}");

            var retint = Common.Ldb.VideoOnlineInfo.DeleteMany(d => d.MediaServerId.Equals(mediaServerId));

            Logger.Debug(
                $"[{Common.LoggerHead}]->启动流媒体服务成功->{mediaServerId}->清理此流媒体下媒体流数量:{retint}");


            return ret;
        }

        /// <summary>
        /// 停止流媒体服务器
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static bool ShutdownMediaServer(string mediaServerId, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->终止流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->终止流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->终止流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var ret = mediaServer.KeeperWebApi.ShutdownMediaServer(out rs);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->终止流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            Logger.Info($"[{Common.LoggerHead}]->终止流媒体服务成功->{mediaServerId}->{ret}");

            var retint = Common.Ldb.VideoOnlineInfo.DeleteMany(d => d.MediaServerId.Equals(mediaServerId));

            Logger.Debug(
                $"[{Common.LoggerHead}]->终止流媒体服务成功->{mediaServerId}->清理此流媒体下媒体流数量:{retint}");

            return ret;
        }


        /// <summary>
        /// 重新启动流媒体服务器
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static ResKeeperRestartMediaServer RestartMediaServer(string mediaServerId, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->重启流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->重启流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_AKStreamKeeperNotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_AKStreamKeeperNotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->重启流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var ret = mediaServer.KeeperWebApi.RestartMediaServer(out rs);
            if (ret == null || !rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->重启流媒体服务失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var retint = Common.Ldb.VideoOnlineInfo.DeleteMany(d => d.MediaServerId.Equals(mediaServerId));

            Logger.Debug(
                $"[{Common.LoggerHead}]->重启流媒体服务成功->{mediaServerId}->清理此流媒体下媒体流数量:{retint}");


            Logger.Info($"[{Common.LoggerHead}]->重启流媒体服务成功->{mediaServerId}->{JsonHelper.ToJson(ret)}");

            return ret;
        }

        /// <summary>
        /// 热加载流媒体服务器配置文件
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static bool ReloadMediaServer(string mediaServerId, out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->热加载流媒体服务配置文件失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->热加载流媒体服务配置文件失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->热加载流媒体服务配置文件失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            var ret = mediaServer.KeeperWebApi.ReloadMediaServer(out rs);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->热加载流媒体服务配置文件失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return false;
            }

            Logger.Info($"[{Common.LoggerHead}]->热加载流媒体服务配置文件成功->{mediaServerId}->{ret}");

            return ret;
        }

        /// <summary>
        /// 获取流媒体服务器运行状态
        /// </summary>
        /// <param name="mediaServerId"></param>
        /// <param name="rs"></param>
        /// <returns></returns>
        public static ResKeeperCheckMediaServerRunning CheckMediaServerRunning(string mediaServerId,
            out ResponseStruct rs)
        {
            rs = new ResponseStruct()
            {
                Code = ErrorNumber.None,
                Message = ErrorMessage.ErrorDic![ErrorNumber.None],
            };
            if (UtilsHelper.StringIsNullEx(mediaServerId))
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.Sys_ParamsIsNotRight,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.Sys_ParamsIsNotRight],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检测流媒体服务器运行状态失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var mediaServer = Common.MediaServerList.FindLast(x => x.MediaServerId.Trim().Equals(mediaServerId.Trim()));
            if (mediaServer == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_InstanceIsNull,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_InstanceIsNull],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检测流媒体服务器运行状态失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            if (mediaServer.KeeperWebApi == null || mediaServer.IsKeeperRunning == false ||
                mediaServer.IsMediaServerRunning == false || mediaServer.WebApiHelper == null)
            {
                rs = new ResponseStruct()
                {
                    Code = ErrorNumber.MediaServer_NotRunning,
                    Message = ErrorMessage.ErrorDic![ErrorNumber.MediaServer_NotRunning],
                };
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检测流媒体服务器运行状态失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            var ret = mediaServer.KeeperWebApi.CheckMediaServerRunning(out rs);
            if (!rs.Code.Equals(ErrorNumber.None))
            {
                Logger.Warn(
                    $"[{Common.LoggerHead}]->检测流媒体服务器运行状态失败->{mediaServerId}->{JsonHelper.ToJson(rs, Formatting.Indented)}");

                return null;
            }

            Logger.Info(
                $"[{Common.LoggerHead}]->检测流媒体服务器运行状态成功->{mediaServerId}->{JsonHelper.ToJson(ret, Formatting.Indented)}");

            return ret;
        }
    }
}