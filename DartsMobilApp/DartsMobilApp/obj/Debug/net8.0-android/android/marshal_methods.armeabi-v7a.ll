; ModuleID = 'marshal_methods.armeabi-v7a.ll'
source_filename = "marshal_methods.armeabi-v7a.ll"
target datalayout = "e-m:e-p:32:32-Fi8-i64:64-v128:64:128-a:0:32-n32-S64"
target triple = "armv7-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [330 x ptr] zeroinitializer, align 4

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [654 x i32] [
	i32 2616222, ; 0: System.Net.NetworkInformation.dll => 0x27eb9e => 68
	i32 10166715, ; 1: System.Net.NameResolution.dll => 0x9b21bb => 67
	i32 15721112, ; 2: System.Runtime.Intrinsics.dll => 0xefe298 => 108
	i32 32687329, ; 3: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 247
	i32 34715100, ; 4: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 281
	i32 34839235, ; 5: System.IO.FileSystem.DriveInfo => 0x2139ac3 => 48
	i32 39485524, ; 6: System.Net.WebSockets.dll => 0x25a8054 => 80
	i32 42639949, ; 7: System.Threading.Thread => 0x28aa24d => 145
	i32 66541672, ; 8: System.Diagnostics.StackTrace => 0x3f75868 => 30
	i32 67008169, ; 9: zh-Hant\Microsoft.Maui.Controls.resources => 0x3fe76a9 => 322
	i32 68219467, ; 10: System.Security.Cryptography.Primitives => 0x410f24b => 124
	i32 72070932, ; 11: Microsoft.Maui.Graphics.dll => 0x44bb714 => 199
	i32 82292897, ; 12: System.Runtime.CompilerServices.VisualC.dll => 0x4e7b0a1 => 102
	i32 101534019, ; 13: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 265
	i32 117431740, ; 14: System.Runtime.InteropServices => 0x6ffddbc => 107
	i32 120558881, ; 15: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 265
	i32 122350210, ; 16: System.Threading.Channels.dll => 0x74aea82 => 139
	i32 134690465, ; 17: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 285
	i32 142721839, ; 18: System.Net.WebHeaderCollection => 0x881c32f => 77
	i32 149972175, ; 19: System.Security.Cryptography.Primitives.dll => 0x8f064cf => 124
	i32 159306688, ; 20: System.ComponentModel.Annotations => 0x97ed3c0 => 13
	i32 165246403, ; 21: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 221
	i32 176265551, ; 22: System.ServiceProcess => 0xa81994f => 132
	i32 182336117, ; 23: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 267
	i32 184328833, ; 24: System.ValueTuple.dll => 0xafca281 => 151
	i32 195452805, ; 25: vi/Microsoft.Maui.Controls.resources.dll => 0xba65f85 => 319
	i32 199333315, ; 26: zh-HK/Microsoft.Maui.Controls.resources.dll => 0xbe195c3 => 320
	i32 205061960, ; 27: System.ComponentModel => 0xc38ff48 => 18
	i32 209399409, ; 28: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 219
	i32 220171995, ; 29: System.Diagnostics.Debug => 0xd1f8edb => 26
	i32 221063263, ; 30: Microsoft.AspNetCore.Http.Connections.Client => 0xd2d285f => 178
	i32 230216969, ; 31: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 241
	i32 230752869, ; 32: Microsoft.CSharp.dll => 0xdc10265 => 1
	i32 231409092, ; 33: System.Linq.Parallel => 0xdcb05c4 => 59
	i32 231814094, ; 34: System.Globalization => 0xdd133ce => 42
	i32 246610117, ; 35: System.Reflection.Emit.Lightweight => 0xeb2f8c5 => 91
	i32 261689757, ; 36: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 224
	i32 276479776, ; 37: System.Threading.Timer.dll => 0x107abf20 => 147
	i32 278686392, ; 38: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 243
	i32 280482487, ; 39: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 240
	i32 280992041, ; 40: cs/Microsoft.Maui.Controls.resources.dll => 0x10bf9929 => 291
	i32 291076382, ; 41: System.IO.Pipes.AccessControl.dll => 0x1159791e => 54
	i32 298918909, ; 42: System.Net.Ping.dll => 0x11d123fd => 69
	i32 317674968, ; 43: vi\Microsoft.Maui.Controls.resources => 0x12ef55d8 => 319
	i32 318968648, ; 44: Xamarin.AndroidX.Activity.dll => 0x13031348 => 210
	i32 321597661, ; 45: System.Numerics => 0x132b30dd => 83
	i32 336156722, ; 46: ja/Microsoft.Maui.Controls.resources.dll => 0x14095832 => 304
	i32 342366114, ; 47: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 242
	i32 347068432, ; 48: SQLitePCLRaw.lib.e_sqlite3.android.dll => 0x14afd810 => 203
	i32 348048101, ; 49: Microsoft.AspNetCore.Http.Connections.Common.dll => 0x14becae5 => 179
	i32 356389973, ; 50: it/Microsoft.Maui.Controls.resources.dll => 0x153e1455 => 303
	i32 360082299, ; 51: System.ServiceModel.Web => 0x15766b7b => 131
	i32 367780167, ; 52: System.IO.Pipes => 0x15ebe147 => 55
	i32 374914964, ; 53: System.Transactions.Local => 0x1658bf94 => 149
	i32 375677976, ; 54: System.Net.ServicePoint.dll => 0x16646418 => 74
	i32 379916513, ; 55: System.Threading.Thread.dll => 0x16a510e1 => 145
	i32 385762202, ; 56: System.Memory.dll => 0x16fe439a => 62
	i32 392610295, ; 57: System.Threading.ThreadPool.dll => 0x1766c1f7 => 146
	i32 395744057, ; 58: _Microsoft.Android.Resource.Designer => 0x17969339 => 326
	i32 403441872, ; 59: WindowsBase => 0x180c08d0 => 165
	i32 435591531, ; 60: sv/Microsoft.Maui.Controls.resources.dll => 0x19f6996b => 315
	i32 441335492, ; 61: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 225
	i32 442565967, ; 62: System.Collections => 0x1a61054f => 12
	i32 450948140, ; 63: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 238
	i32 451504562, ; 64: System.Security.Cryptography.X509Certificates => 0x1ae969b2 => 125
	i32 456227837, ; 65: System.Web.HttpUtility.dll => 0x1b317bfd => 152
	i32 458494020, ; 66: Microsoft.AspNetCore.SignalR.Common.dll => 0x1b541044 => 182
	i32 459347974, ; 67: System.Runtime.Serialization.Primitives.dll => 0x1b611806 => 113
	i32 465846621, ; 68: mscorlib => 0x1bc4415d => 166
	i32 469710990, ; 69: System.dll => 0x1bff388e => 164
	i32 476646585, ; 70: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 240
	i32 486930444, ; 71: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 253
	i32 498788369, ; 72: System.ObjectModel => 0x1dbae811 => 84
	i32 500358224, ; 73: id/Microsoft.Maui.Controls.resources.dll => 0x1dd2dc50 => 302
	i32 503918385, ; 74: fi/Microsoft.Maui.Controls.resources.dll => 0x1e092f31 => 296
	i32 513247710, ; 75: Microsoft.Extensions.Primitives.dll => 0x1e9789de => 193
	i32 526420162, ; 76: System.Transactions.dll => 0x1f6088c2 => 150
	i32 527452488, ; 77: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 285
	i32 530272170, ; 78: System.Linq.Queryable => 0x1f9b4faa => 60
	i32 539058512, ; 79: Microsoft.Extensions.Logging => 0x20216150 => 189
	i32 540030774, ; 80: System.IO.FileSystem.dll => 0x20303736 => 51
	i32 545304856, ; 81: System.Runtime.Extensions => 0x2080b118 => 103
	i32 546455878, ; 82: System.Runtime.Serialization.Xml => 0x20924146 => 114
	i32 549171840, ; 83: System.Globalization.Calendars => 0x20bbb280 => 40
	i32 557405415, ; 84: Jsr305Binding => 0x213954e7 => 278
	i32 569601784, ; 85: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 276
	i32 577335427, ; 86: System.Security.Cryptography.Cng => 0x22697083 => 120
	i32 592146354, ; 87: pt-BR/Microsoft.Maui.Controls.resources.dll => 0x234b6fb2 => 310
	i32 597488923, ; 88: CommunityToolkit.Maui => 0x239cf51b => 173
	i32 601371474, ; 89: System.IO.IsolatedStorage.dll => 0x23d83352 => 52
	i32 605376203, ; 90: System.IO.Compression.FileSystem => 0x24154ecb => 44
	i32 613668793, ; 91: System.Security.Cryptography.Algorithms => 0x2493d7b9 => 119
	i32 627609679, ; 92: Xamarin.AndroidX.CustomView => 0x2568904f => 230
	i32 627931235, ; 93: nl\Microsoft.Maui.Controls.resources => 0x256d7863 => 308
	i32 639843206, ; 94: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 236
	i32 643868501, ; 95: System.Net => 0x2660a755 => 81
	i32 662205335, ; 96: System.Text.Encodings.Web.dll => 0x27787397 => 136
	i32 663517072, ; 97: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 272
	i32 666292255, ; 98: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 217
	i32 672442732, ; 99: System.Collections.Concurrent => 0x2814a96c => 8
	i32 683518922, ; 100: System.Net.Security => 0x28bdabca => 73
	i32 688181140, ; 101: ca/Microsoft.Maui.Controls.resources.dll => 0x2904cf94 => 290
	i32 690569205, ; 102: System.Xml.Linq.dll => 0x29293ff5 => 155
	i32 691348768, ; 103: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 287
	i32 693804605, ; 104: System.Windows => 0x295a9e3d => 154
	i32 699345723, ; 105: System.Reflection.Emit => 0x29af2b3b => 92
	i32 700284507, ; 106: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 282
	i32 700358131, ; 107: System.IO.Compression.ZipFile => 0x29be9df3 => 45
	i32 706645707, ; 108: ko/Microsoft.Maui.Controls.resources.dll => 0x2a1e8ecb => 305
	i32 709557578, ; 109: de/Microsoft.Maui.Controls.resources.dll => 0x2a4afd4a => 293
	i32 720511267, ; 110: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 286
	i32 722857257, ; 111: System.Runtime.Loader.dll => 0x2b15ed29 => 109
	i32 735137430, ; 112: System.Security.SecureString.dll => 0x2bd14e96 => 129
	i32 748832960, ; 113: SQLitePCLRaw.batteries_v2 => 0x2ca248c0 => 201
	i32 752232764, ; 114: System.Diagnostics.Contracts.dll => 0x2cd6293c => 25
	i32 755313932, ; 115: Xamarin.Android.Glide.Annotations.dll => 0x2d052d0c => 207
	i32 759454413, ; 116: System.Net.Requests => 0x2d445acd => 72
	i32 762598435, ; 117: System.IO.Pipes.dll => 0x2d745423 => 55
	i32 775507847, ; 118: System.IO.Compression => 0x2e394f87 => 46
	i32 777317022, ; 119: sk\Microsoft.Maui.Controls.resources => 0x2e54ea9e => 314
	i32 789151979, ; 120: Microsoft.Extensions.Options => 0x2f0980eb => 192
	i32 790371945, ; 121: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 231
	i32 804715423, ; 122: System.Data.Common => 0x2ff6fb9f => 22
	i32 807930345, ; 123: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx.dll => 0x302809e9 => 245
	i32 823281589, ; 124: System.Private.Uri.dll => 0x311247b5 => 86
	i32 830298997, ; 125: System.IO.Compression.Brotli => 0x317d5b75 => 43
	i32 832635846, ; 126: System.Xml.XPath.dll => 0x31a103c6 => 160
	i32 832711436, ; 127: Microsoft.AspNetCore.SignalR.Protocols.Json.dll => 0x31a22b0c => 183
	i32 834051424, ; 128: System.Net.Quic => 0x31b69d60 => 71
	i32 843511501, ; 129: Xamarin.AndroidX.Print => 0x3246f6cd => 258
	i32 873119928, ; 130: Microsoft.VisualBasic => 0x340ac0b8 => 3
	i32 877678880, ; 131: System.Globalization.dll => 0x34505120 => 42
	i32 878954865, ; 132: System.Net.Http.Json => 0x3463c971 => 63
	i32 904024072, ; 133: System.ComponentModel.Primitives.dll => 0x35e25008 => 16
	i32 911108515, ; 134: System.IO.MemoryMappedFiles.dll => 0x364e69a3 => 53
	i32 926902833, ; 135: tr/Microsoft.Maui.Controls.resources.dll => 0x373f6a31 => 317
	i32 928116545, ; 136: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 281
	i32 952186615, ; 137: System.Runtime.InteropServices.JavaScript.dll => 0x38c136f7 => 105
	i32 956575887, ; 138: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 286
	i32 966729478, ; 139: Xamarin.Google.Crypto.Tink.Android => 0x399f1f06 => 279
	i32 967690846, ; 140: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 242
	i32 975236339, ; 141: System.Diagnostics.Tracing => 0x3a20ecf3 => 34
	i32 975874589, ; 142: System.Xml.XDocument => 0x3a2aaa1d => 158
	i32 986514023, ; 143: System.Private.DataContractSerialization.dll => 0x3acd0267 => 85
	i32 987214855, ; 144: System.Diagnostics.Tools => 0x3ad7b407 => 32
	i32 992768348, ; 145: System.Collections.dll => 0x3b2c715c => 12
	i32 994442037, ; 146: System.IO.FileSystem => 0x3b45fb35 => 51
	i32 1001831731, ; 147: System.IO.UnmanagedMemoryStream.dll => 0x3bb6bd33 => 56
	i32 1012816738, ; 148: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 262
	i32 1019214401, ; 149: System.Drawing => 0x3cbffa41 => 36
	i32 1028951442, ; 150: Microsoft.Extensions.DependencyInjection.Abstractions => 0x3d548d92 => 187
	i32 1029334545, ; 151: da/Microsoft.Maui.Controls.resources.dll => 0x3d5a6611 => 292
	i32 1031528504, ; 152: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 280
	i32 1035644815, ; 153: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 215
	i32 1036536393, ; 154: System.Drawing.Primitives.dll => 0x3dc84a49 => 35
	i32 1044663988, ; 155: System.Linq.Expressions.dll => 0x3e444eb4 => 58
	i32 1052210849, ; 156: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 249
	i32 1058641855, ; 157: Microsoft.AspNetCore.Http.Connections.Common => 0x3f1997bf => 179
	i32 1067306892, ; 158: GoogleGson => 0x3f9dcf8c => 176
	i32 1082857460, ; 159: System.ComponentModel.TypeConverter => 0x408b17f4 => 17
	i32 1084122840, ; 160: Xamarin.Kotlin.StdLib => 0x409e66d8 => 283
	i32 1098259244, ; 161: System => 0x41761b2c => 164
	i32 1118262833, ; 162: ko\Microsoft.Maui.Controls.resources => 0x42a75631 => 305
	i32 1121599056, ; 163: Xamarin.AndroidX.Lifecycle.Runtime.Ktx.dll => 0x42da3e50 => 248
	i32 1127624469, ; 164: Microsoft.Extensions.Logging.Debug => 0x43362f15 => 191
	i32 1149092582, ; 165: Xamarin.AndroidX.Window => 0x447dc2e6 => 275
	i32 1168523401, ; 166: pt\Microsoft.Maui.Controls.resources => 0x45a64089 => 311
	i32 1170634674, ; 167: System.Web.dll => 0x45c677b2 => 153
	i32 1175144683, ; 168: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 271
	i32 1178241025, ; 169: Xamarin.AndroidX.Navigation.Runtime.dll => 0x463a8801 => 256
	i32 1203215381, ; 170: pl/Microsoft.Maui.Controls.resources.dll => 0x47b79c15 => 309
	i32 1204270330, ; 171: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 217
	i32 1208641965, ; 172: System.Diagnostics.Process => 0x480a69ad => 29
	i32 1214827643, ; 173: CommunityToolkit.Mvvm => 0x4868cc7b => 175
	i32 1219128291, ; 174: System.IO.IsolatedStorage => 0x48aa6be3 => 52
	i32 1233093933, ; 175: Microsoft.AspNetCore.SignalR.Client.Core.dll => 0x497f852d => 181
	i32 1234928153, ; 176: nb/Microsoft.Maui.Controls.resources.dll => 0x499b8219 => 307
	i32 1243150071, ; 177: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 276
	i32 1253011324, ; 178: Microsoft.Win32.Registry => 0x4aaf6f7c => 5
	i32 1260983243, ; 179: cs\Microsoft.Maui.Controls.resources => 0x4b2913cb => 291
	i32 1264511973, ; 180: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 266
	i32 1267360935, ; 181: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 270
	i32 1273260888, ; 182: Xamarin.AndroidX.Collection.Ktx => 0x4be46b58 => 222
	i32 1275534314, ; 183: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 287
	i32 1278448581, ; 184: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 214
	i32 1292207520, ; 185: SQLitePCLRaw.core.dll => 0x4d0585a0 => 202
	i32 1293217323, ; 186: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 233
	i32 1309188875, ; 187: System.Private.DataContractSerialization => 0x4e08a30b => 85
	i32 1322716291, ; 188: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 275
	i32 1324164729, ; 189: System.Linq => 0x4eed2679 => 61
	i32 1335329327, ; 190: System.Runtime.Serialization.Json.dll => 0x4f97822f => 112
	i32 1364015309, ; 191: System.IO => 0x514d38cd => 57
	i32 1373134921, ; 192: zh-Hans\Microsoft.Maui.Controls.resources => 0x51d86049 => 321
	i32 1376866003, ; 193: Xamarin.AndroidX.SavedState => 0x52114ed3 => 262
	i32 1379779777, ; 194: System.Resources.ResourceManager => 0x523dc4c1 => 99
	i32 1402170036, ; 195: System.Configuration.dll => 0x53936ab4 => 19
	i32 1406073936, ; 196: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 226
	i32 1408764838, ; 197: System.Runtime.Serialization.Formatters.dll => 0x53f80ba6 => 111
	i32 1411638395, ; 198: System.Runtime.CompilerServices.Unsafe => 0x5423e47b => 101
	i32 1414043276, ; 199: Microsoft.AspNetCore.Connections.Abstractions.dll => 0x5448968c => 177
	i32 1422545099, ; 200: System.Runtime.CompilerServices.VisualC => 0x54ca50cb => 102
	i32 1430672901, ; 201: ar\Microsoft.Maui.Controls.resources => 0x55465605 => 289
	i32 1434145427, ; 202: System.Runtime.Handles => 0x557b5293 => 104
	i32 1435222561, ; 203: Xamarin.Google.Crypto.Tink.Android.dll => 0x558bc221 => 279
	i32 1439761251, ; 204: System.Net.Quic.dll => 0x55d10363 => 71
	i32 1452070440, ; 205: System.Formats.Asn1.dll => 0x568cd628 => 38
	i32 1453312822, ; 206: System.Diagnostics.Tools.dll => 0x569fcb36 => 32
	i32 1457743152, ; 207: System.Runtime.Extensions.dll => 0x56e36530 => 103
	i32 1458022317, ; 208: System.Net.Security.dll => 0x56e7a7ad => 73
	i32 1461004990, ; 209: es\Microsoft.Maui.Controls.resources => 0x57152abe => 295
	i32 1461234159, ; 210: System.Collections.Immutable.dll => 0x5718a9ef => 9
	i32 1461719063, ; 211: System.Security.Cryptography.OpenSsl => 0x57201017 => 123
	i32 1462112819, ; 212: System.IO.Compression.dll => 0x57261233 => 46
	i32 1469204771, ; 213: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 216
	i32 1470490898, ; 214: Microsoft.Extensions.Primitives => 0x57a5e912 => 193
	i32 1479771757, ; 215: System.Collections.Immutable => 0x5833866d => 9
	i32 1480492111, ; 216: System.IO.Compression.Brotli.dll => 0x583e844f => 43
	i32 1487239319, ; 217: Microsoft.Win32.Primitives => 0x58a57897 => 4
	i32 1490025113, ; 218: Xamarin.AndroidX.SavedState.SavedState.Ktx.dll => 0x58cffa99 => 263
	i32 1493001747, ; 219: hi/Microsoft.Maui.Controls.resources.dll => 0x58fd6613 => 299
	i32 1514721132, ; 220: el/Microsoft.Maui.Controls.resources.dll => 0x5a48cf6c => 294
	i32 1536373174, ; 221: System.Diagnostics.TextWriterTraceListener => 0x5b9331b6 => 31
	i32 1543031311, ; 222: System.Text.RegularExpressions.dll => 0x5bf8ca0f => 138
	i32 1543355203, ; 223: System.Reflection.Emit.dll => 0x5bfdbb43 => 92
	i32 1550322496, ; 224: System.Reflection.Extensions.dll => 0x5c680b40 => 93
	i32 1551623176, ; 225: sk/Microsoft.Maui.Controls.resources.dll => 0x5c7be408 => 314
	i32 1565862583, ; 226: System.IO.FileSystem.Primitives => 0x5d552ab7 => 49
	i32 1566207040, ; 227: System.Threading.Tasks.Dataflow.dll => 0x5d5a6c40 => 141
	i32 1573704789, ; 228: System.Runtime.Serialization.Json => 0x5dccd455 => 112
	i32 1580037396, ; 229: System.Threading.Overlapped => 0x5e2d7514 => 140
	i32 1582372066, ; 230: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 232
	i32 1592978981, ; 231: System.Runtime.Serialization.dll => 0x5ef2ee25 => 115
	i32 1597949149, ; 232: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 280
	i32 1601112923, ; 233: System.Xml.Serialization => 0x5f6f0b5b => 157
	i32 1603525486, ; 234: Microsoft.Maui.Controls.HotReload.Forms.dll => 0x5f93db6e => 323
	i32 1604827217, ; 235: System.Net.WebClient => 0x5fa7b851 => 76
	i32 1618516317, ; 236: System.Net.WebSockets.Client.dll => 0x6078995d => 79
	i32 1622152042, ; 237: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 252
	i32 1622358360, ; 238: System.Dynamic.Runtime => 0x60b33958 => 37
	i32 1624863272, ; 239: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 274
	i32 1634654947, ; 240: CommunityToolkit.Maui.Core.dll => 0x616edae3 => 174
	i32 1635184631, ; 241: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 236
	i32 1636350590, ; 242: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 229
	i32 1639515021, ; 243: System.Net.Http.dll => 0x61b9038d => 64
	i32 1639986890, ; 244: System.Text.RegularExpressions => 0x61c036ca => 138
	i32 1641389582, ; 245: System.ComponentModel.EventBasedAsync.dll => 0x61d59e0e => 15
	i32 1657153582, ; 246: System.Runtime => 0x62c6282e => 116
	i32 1658241508, ; 247: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 268
	i32 1658251792, ; 248: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 277
	i32 1670060433, ; 249: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 224
	i32 1675553242, ; 250: System.IO.FileSystem.DriveInfo.dll => 0x63dee9da => 48
	i32 1677501392, ; 251: System.Net.Primitives.dll => 0x63fca3d0 => 70
	i32 1678508291, ; 252: System.Net.WebSockets => 0x640c0103 => 80
	i32 1679769178, ; 253: System.Security.Cryptography => 0x641f3e5a => 126
	i32 1691477237, ; 254: System.Reflection.Metadata => 0x64d1e4f5 => 94
	i32 1696967625, ; 255: System.Security.Cryptography.Csp => 0x6525abc9 => 121
	i32 1698840827, ; 256: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 284
	i32 1701541528, ; 257: System.Diagnostics.Debug.dll => 0x656b7698 => 26
	i32 1711441057, ; 258: SQLitePCLRaw.lib.e_sqlite3.android => 0x660284a1 => 203
	i32 1720223769, ; 259: Xamarin.AndroidX.Lifecycle.LiveData.Core.Ktx => 0x66888819 => 245
	i32 1726116996, ; 260: System.Reflection.dll => 0x66e27484 => 97
	i32 1728033016, ; 261: System.Diagnostics.FileVersionInfo.dll => 0x66ffb0f8 => 28
	i32 1729485958, ; 262: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 220
	i32 1736233607, ; 263: ro/Microsoft.Maui.Controls.resources.dll => 0x677cd287 => 312
	i32 1743415430, ; 264: ca\Microsoft.Maui.Controls.resources => 0x67ea6886 => 290
	i32 1744735666, ; 265: System.Transactions.Local.dll => 0x67fe8db2 => 149
	i32 1746115085, ; 266: System.IO.Pipelines.dll => 0x68139a0d => 205
	i32 1746316138, ; 267: Mono.Android.Export => 0x6816ab6a => 169
	i32 1750313021, ; 268: Microsoft.Win32.Primitives.dll => 0x6853a83d => 4
	i32 1758240030, ; 269: System.Resources.Reader.dll => 0x68cc9d1e => 98
	i32 1763938596, ; 270: System.Diagnostics.TraceSource.dll => 0x69239124 => 33
	i32 1765942094, ; 271: System.Reflection.Extensions => 0x6942234e => 93
	i32 1766324549, ; 272: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 267
	i32 1770582343, ; 273: Microsoft.Extensions.Logging.dll => 0x6988f147 => 189
	i32 1776026572, ; 274: System.Core.dll => 0x69dc03cc => 21
	i32 1777075843, ; 275: System.Globalization.Extensions.dll => 0x69ec0683 => 41
	i32 1780572499, ; 276: Mono.Android.Runtime.dll => 0x6a216153 => 170
	i32 1782862114, ; 277: ms\Microsoft.Maui.Controls.resources => 0x6a445122 => 306
	i32 1788241197, ; 278: Xamarin.AndroidX.Fragment => 0x6a96652d => 238
	i32 1793755602, ; 279: he\Microsoft.Maui.Controls.resources => 0x6aea89d2 => 298
	i32 1808609942, ; 280: Xamarin.AndroidX.Loader => 0x6bcd3296 => 252
	i32 1813058853, ; 281: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 283
	i32 1813201214, ; 282: Xamarin.Google.Android.Material => 0x6c13413e => 277
	i32 1818569960, ; 283: Xamarin.AndroidX.Navigation.UI.dll => 0x6c652ce8 => 257
	i32 1818787751, ; 284: Microsoft.VisualBasic.Core => 0x6c687fa7 => 2
	i32 1824175904, ; 285: System.Text.Encoding.Extensions => 0x6cbab720 => 134
	i32 1824722060, ; 286: System.Runtime.Serialization.Formatters => 0x6cc30c8c => 111
	i32 1827303595, ; 287: Microsoft.VisualStudio.DesignTools.TapContract => 0x6cea70ab => 325
	i32 1828688058, ; 288: Microsoft.Extensions.Logging.Abstractions.dll => 0x6cff90ba => 190
	i32 1842015223, ; 289: uk/Microsoft.Maui.Controls.resources.dll => 0x6dcaebf7 => 318
	i32 1847515442, ; 290: Xamarin.Android.Glide.Annotations => 0x6e1ed932 => 207
	i32 1853025655, ; 291: sv\Microsoft.Maui.Controls.resources => 0x6e72ed77 => 315
	i32 1858542181, ; 292: System.Linq.Expressions => 0x6ec71a65 => 58
	i32 1870277092, ; 293: System.Reflection.Primitives => 0x6f7a29e4 => 95
	i32 1875935024, ; 294: fr\Microsoft.Maui.Controls.resources => 0x6fd07f30 => 297
	i32 1879696579, ; 295: System.Formats.Tar.dll => 0x7009e4c3 => 39
	i32 1885316902, ; 296: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 218
	i32 1885918049, ; 297: Microsoft.VisualStudio.DesignTools.TapContract.dll => 0x7068d361 => 325
	i32 1888955245, ; 298: System.Diagnostics.Contracts => 0x70972b6d => 25
	i32 1889954781, ; 299: System.Reflection.Metadata.dll => 0x70a66bdd => 94
	i32 1898237753, ; 300: System.Reflection.DispatchProxy => 0x7124cf39 => 89
	i32 1900610850, ; 301: System.Resources.ResourceManager.dll => 0x71490522 => 99
	i32 1910275211, ; 302: System.Collections.NonGeneric.dll => 0x71dc7c8b => 10
	i32 1939592360, ; 303: System.Private.Xml.Linq => 0x739bd4a8 => 87
	i32 1945717188, ; 304: Microsoft.AspNetCore.SignalR.Client.Core => 0x73f949c4 => 181
	i32 1956758971, ; 305: System.Resources.Writer => 0x74a1c5bb => 100
	i32 1961813231, ; 306: Xamarin.AndroidX.Security.SecurityCrypto.dll => 0x74eee4ef => 264
	i32 1967334205, ; 307: Microsoft.AspNetCore.SignalR.Common => 0x7543233d => 182
	i32 1968388702, ; 308: Microsoft.Extensions.Configuration.dll => 0x75533a5e => 184
	i32 1983156543, ; 309: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 284
	i32 1985761444, ; 310: Xamarin.Android.Glide.GifDecoder => 0x765c50a4 => 209
	i32 2003115576, ; 311: el\Microsoft.Maui.Controls.resources => 0x77651e38 => 294
	i32 2011961780, ; 312: System.Buffers.dll => 0x77ec19b4 => 7
	i32 2019465201, ; 313: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 249
	i32 2025202353, ; 314: ar/Microsoft.Maui.Controls.resources.dll => 0x78b622b1 => 289
	i32 2031763787, ; 315: Xamarin.Android.Glide => 0x791a414b => 206
	i32 2045470958, ; 316: System.Private.Xml => 0x79eb68ee => 88
	i32 2055257422, ; 317: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 244
	i32 2060060697, ; 318: System.Windows.dll => 0x7aca0819 => 154
	i32 2066184531, ; 319: de\Microsoft.Maui.Controls.resources => 0x7b277953 => 293
	i32 2070888862, ; 320: System.Diagnostics.TraceSource => 0x7b6f419e => 33
	i32 2079903147, ; 321: System.Runtime.dll => 0x7bf8cdab => 116
	i32 2090596640, ; 322: System.Numerics.Vectors => 0x7c9bf920 => 82
	i32 2103459038, ; 323: SQLitePCLRaw.provider.e_sqlite3.dll => 0x7d603cde => 204
	i32 2127167465, ; 324: System.Console => 0x7ec9ffe9 => 20
	i32 2142473426, ; 325: System.Collections.Specialized => 0x7fb38cd2 => 11
	i32 2143790110, ; 326: System.Xml.XmlSerializer.dll => 0x7fc7a41e => 162
	i32 2146852085, ; 327: Microsoft.VisualBasic.dll => 0x7ff65cf5 => 3
	i32 2159891885, ; 328: Microsoft.Maui => 0x80bd55ad => 197
	i32 2169148018, ; 329: hu\Microsoft.Maui.Controls.resources => 0x814a9272 => 301
	i32 2181898931, ; 330: Microsoft.Extensions.Options.dll => 0x820d22b3 => 192
	i32 2192057212, ; 331: Microsoft.Extensions.Logging.Abstractions => 0x82a8237c => 190
	i32 2193016926, ; 332: System.ObjectModel.dll => 0x82b6c85e => 84
	i32 2201107256, ; 333: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 288
	i32 2201231467, ; 334: System.Net.Http => 0x8334206b => 64
	i32 2207618523, ; 335: it\Microsoft.Maui.Controls.resources => 0x839595db => 303
	i32 2217644978, ; 336: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 271
	i32 2222056684, ; 337: System.Threading.Tasks.Parallel => 0x8471e4ec => 143
	i32 2229158877, ; 338: Microsoft.Extensions.Features.dll => 0x84de43dd => 188
	i32 2244775296, ; 339: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 253
	i32 2252106437, ; 340: System.Xml.Serialization.dll => 0x863c6ac5 => 157
	i32 2256313426, ; 341: System.Globalization.Extensions => 0x867c9c52 => 41
	i32 2265110946, ; 342: System.Security.AccessControl.dll => 0x8702d9a2 => 117
	i32 2266799131, ; 343: Microsoft.Extensions.Configuration.Abstractions => 0x871c9c1b => 185
	i32 2267999099, ; 344: Xamarin.Android.Glide.DiskLruCache.dll => 0x872eeb7b => 208
	i32 2270573516, ; 345: fr/Microsoft.Maui.Controls.resources.dll => 0x875633cc => 297
	i32 2279755925, ; 346: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 260
	i32 2293034957, ; 347: System.ServiceModel.Web.dll => 0x88acefcd => 131
	i32 2295906218, ; 348: System.Net.Sockets => 0x88d8bfaa => 75
	i32 2298471582, ; 349: System.Net.Mail => 0x88ffe49e => 66
	i32 2303942373, ; 350: nb\Microsoft.Maui.Controls.resources => 0x89535ee5 => 307
	i32 2305521784, ; 351: System.Private.CoreLib.dll => 0x896b7878 => 172
	i32 2315684594, ; 352: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 212
	i32 2319144366, ; 353: Microsoft.AspNetCore.SignalR.Client => 0x8a3b55ae => 180
	i32 2320631194, ; 354: System.Threading.Tasks.Parallel.dll => 0x8a52059a => 143
	i32 2340441535, ; 355: System.Runtime.InteropServices.RuntimeInformation.dll => 0x8b804dbf => 106
	i32 2344264397, ; 356: System.ValueTuple => 0x8bbaa2cd => 151
	i32 2353062107, ; 357: System.Net.Primitives => 0x8c40e0db => 70
	i32 2368005991, ; 358: System.Xml.ReaderWriter.dll => 0x8d24e767 => 156
	i32 2371007202, ; 359: Microsoft.Extensions.Configuration => 0x8d52b2e2 => 184
	i32 2378619854, ; 360: System.Security.Cryptography.Csp.dll => 0x8dc6dbce => 121
	i32 2383496789, ; 361: System.Security.Principal.Windows.dll => 0x8e114655 => 127
	i32 2395872292, ; 362: id\Microsoft.Maui.Controls.resources => 0x8ece1c24 => 302
	i32 2401565422, ; 363: System.Web.HttpUtility => 0x8f24faee => 152
	i32 2403452196, ; 364: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 235
	i32 2409983638, ; 365: Microsoft.VisualStudio.DesignTools.MobileTapContracts.dll => 0x8fa56e96 => 324
	i32 2418651478, ; 366: DartsMobilApp.dll => 0x9029b156 => 0
	i32 2421380589, ; 367: System.Threading.Tasks.Dataflow => 0x905355ed => 141
	i32 2423080555, ; 368: Xamarin.AndroidX.Collection.Ktx.dll => 0x906d466b => 222
	i32 2427813419, ; 369: hi\Microsoft.Maui.Controls.resources => 0x90b57e2b => 299
	i32 2435356389, ; 370: System.Console.dll => 0x912896e5 => 20
	i32 2435904999, ; 371: System.ComponentModel.DataAnnotations.dll => 0x9130f5e7 => 14
	i32 2454642406, ; 372: System.Text.Encoding.dll => 0x924edee6 => 135
	i32 2458678730, ; 373: System.Net.Sockets.dll => 0x928c75ca => 75
	i32 2459001652, ; 374: System.Linq.Parallel.dll => 0x92916334 => 59
	i32 2465273461, ; 375: SQLitePCLRaw.batteries_v2.dll => 0x92f11675 => 201
	i32 2465532216, ; 376: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 225
	i32 2471841756, ; 377: netstandard.dll => 0x93554fdc => 167
	i32 2475788418, ; 378: Java.Interop.dll => 0x93918882 => 168
	i32 2480646305, ; 379: Microsoft.Maui.Controls => 0x93dba8a1 => 195
	i32 2483903535, ; 380: System.ComponentModel.EventBasedAsync => 0x940d5c2f => 15
	i32 2484371297, ; 381: System.Net.ServicePoint => 0x94147f61 => 74
	i32 2490993605, ; 382: System.AppContext.dll => 0x94798bc5 => 6
	i32 2501346920, ; 383: System.Data.DataSetExtensions => 0x95178668 => 23
	i32 2505896520, ; 384: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 247
	i32 2522472828, ; 385: Xamarin.Android.Glide.dll => 0x9659e17c => 206
	i32 2538310050, ; 386: System.Reflection.Emit.Lightweight.dll => 0x974b89a2 => 91
	i32 2550873716, ; 387: hr\Microsoft.Maui.Controls.resources => 0x980b3e74 => 300
	i32 2562349572, ; 388: Microsoft.CSharp => 0x98ba5a04 => 1
	i32 2570120770, ; 389: System.Text.Encodings.Web => 0x9930ee42 => 136
	i32 2581783588, ; 390: Xamarin.AndroidX.Lifecycle.Runtime.Ktx => 0x99e2e424 => 248
	i32 2581819634, ; 391: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 270
	i32 2585220780, ; 392: System.Text.Encoding.Extensions.dll => 0x9a1756ac => 134
	i32 2585805581, ; 393: System.Net.Ping => 0x9a20430d => 69
	i32 2589602615, ; 394: System.Threading.ThreadPool => 0x9a5a3337 => 146
	i32 2593496499, ; 395: pl\Microsoft.Maui.Controls.resources => 0x9a959db3 => 309
	i32 2605712449, ; 396: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 288
	i32 2615233544, ; 397: Xamarin.AndroidX.Fragment.Ktx => 0x9be14c08 => 239
	i32 2616218305, ; 398: Microsoft.Extensions.Logging.Debug.dll => 0x9bf052c1 => 191
	i32 2617129537, ; 399: System.Private.Xml.dll => 0x9bfe3a41 => 88
	i32 2618712057, ; 400: System.Reflection.TypeExtensions.dll => 0x9c165ff9 => 96
	i32 2620871830, ; 401: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 229
	i32 2624644809, ; 402: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 234
	i32 2626831493, ; 403: ja\Microsoft.Maui.Controls.resources => 0x9c924485 => 304
	i32 2627185994, ; 404: System.Diagnostics.TextWriterTraceListener.dll => 0x9c97ad4a => 31
	i32 2629843544, ; 405: System.IO.Compression.ZipFile.dll => 0x9cc03a58 => 45
	i32 2633051222, ; 406: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 243
	i32 2637500010, ; 407: Microsoft.Extensions.Features => 0x9d350e6a => 188
	i32 2663391936, ; 408: Xamarin.Android.Glide.DiskLruCache => 0x9ec022c0 => 208
	i32 2663698177, ; 409: System.Runtime.Loader => 0x9ec4cf01 => 109
	i32 2664396074, ; 410: System.Xml.XDocument.dll => 0x9ecf752a => 158
	i32 2665622720, ; 411: System.Drawing.Primitives => 0x9ee22cc0 => 35
	i32 2676780864, ; 412: System.Data.Common.dll => 0x9f8c6f40 => 22
	i32 2686887180, ; 413: System.Runtime.Serialization.Xml.dll => 0xa026a50c => 114
	i32 2693849962, ; 414: System.IO.dll => 0xa090e36a => 57
	i32 2701096212, ; 415: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 268
	i32 2715334215, ; 416: System.Threading.Tasks.dll => 0xa1d8b647 => 144
	i32 2717744543, ; 417: System.Security.Claims => 0xa1fd7d9f => 118
	i32 2719963679, ; 418: System.Security.Cryptography.Cng.dll => 0xa21f5a1f => 120
	i32 2724373263, ; 419: System.Runtime.Numerics.dll => 0xa262a30f => 110
	i32 2732626843, ; 420: Xamarin.AndroidX.Activity => 0xa2e0939b => 210
	i32 2735172069, ; 421: System.Threading.Channels => 0xa30769e5 => 139
	i32 2737747696, ; 422: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 216
	i32 2740948882, ; 423: System.IO.Pipes.AccessControl => 0xa35f8f92 => 54
	i32 2748088231, ; 424: System.Runtime.InteropServices.JavaScript => 0xa3cc7fa7 => 105
	i32 2752995522, ; 425: pt-BR\Microsoft.Maui.Controls.resources => 0xa41760c2 => 310
	i32 2758225723, ; 426: Microsoft.Maui.Controls.Xaml => 0xa4672f3b => 196
	i32 2759616592, ; 427: DartsMobilApp => 0xa47c6850 => 0
	i32 2764765095, ; 428: Microsoft.Maui.dll => 0xa4caf7a7 => 197
	i32 2765824710, ; 429: System.Text.Encoding.CodePages.dll => 0xa4db22c6 => 133
	i32 2770495804, ; 430: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 282
	i32 2778768386, ; 431: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 273
	i32 2779977773, ; 432: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 261
	i32 2785988530, ; 433: th\Microsoft.Maui.Controls.resources => 0xa60ecfb2 => 316
	i32 2788224221, ; 434: Xamarin.AndroidX.Fragment.Ktx.dll => 0xa630ecdd => 239
	i32 2801831435, ; 435: Microsoft.Maui.Graphics => 0xa7008e0b => 199
	i32 2803228030, ; 436: System.Xml.XPath.XDocument.dll => 0xa715dd7e => 159
	i32 2806116107, ; 437: es/Microsoft.Maui.Controls.resources.dll => 0xa741ef0b => 295
	i32 2810250172, ; 438: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 226
	i32 2819470561, ; 439: System.Xml.dll => 0xa80db4e1 => 163
	i32 2821205001, ; 440: System.ServiceProcess.dll => 0xa8282c09 => 132
	i32 2821294376, ; 441: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 261
	i32 2824502124, ; 442: System.Xml.XmlDocument => 0xa85a7b6c => 161
	i32 2831556043, ; 443: nl/Microsoft.Maui.Controls.resources.dll => 0xa8c61dcb => 308
	i32 2838993487, ; 444: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx.dll => 0xa9379a4f => 250
	i32 2849599387, ; 445: System.Threading.Overlapped.dll => 0xa9d96f9b => 140
	i32 2853208004, ; 446: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 273
	i32 2855708567, ; 447: Xamarin.AndroidX.Transition => 0xaa36a797 => 269
	i32 2861098320, ; 448: Mono.Android.Export.dll => 0xaa88e550 => 169
	i32 2861189240, ; 449: Microsoft.Maui.Essentials => 0xaa8a4878 => 198
	i32 2868488919, ; 450: CommunityToolkit.Maui.Core => 0xaaf9aad7 => 174
	i32 2870099610, ; 451: Xamarin.AndroidX.Activity.Ktx.dll => 0xab123e9a => 211
	i32 2875164099, ; 452: Jsr305Binding.dll => 0xab5f85c3 => 278
	i32 2875220617, ; 453: System.Globalization.Calendars.dll => 0xab606289 => 40
	i32 2875347124, ; 454: Microsoft.AspNetCore.Http.Connections.Client.dll => 0xab6250b4 => 178
	i32 2884993177, ; 455: Xamarin.AndroidX.ExifInterface => 0xabf58099 => 237
	i32 2887636118, ; 456: System.Net.dll => 0xac1dd496 => 81
	i32 2899753641, ; 457: System.IO.UnmanagedMemoryStream => 0xacd6baa9 => 56
	i32 2900621748, ; 458: System.Dynamic.Runtime.dll => 0xace3f9b4 => 37
	i32 2901442782, ; 459: System.Reflection => 0xacf080de => 97
	i32 2905242038, ; 460: mscorlib.dll => 0xad2a79b6 => 166
	i32 2909740682, ; 461: System.Private.CoreLib => 0xad6f1e8a => 172
	i32 2916838712, ; 462: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 274
	i32 2919462931, ; 463: System.Numerics.Vectors.dll => 0xae037813 => 82
	i32 2921128767, ; 464: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 213
	i32 2936416060, ; 465: System.Resources.Reader => 0xaf06273c => 98
	i32 2940926066, ; 466: System.Diagnostics.StackTrace.dll => 0xaf4af872 => 30
	i32 2942453041, ; 467: System.Xml.XPath.XDocument => 0xaf624531 => 159
	i32 2959614098, ; 468: System.ComponentModel.dll => 0xb0682092 => 18
	i32 2968338931, ; 469: System.Security.Principal.Windows => 0xb0ed41f3 => 127
	i32 2972252294, ; 470: System.Security.Cryptography.Algorithms.dll => 0xb128f886 => 119
	i32 2978675010, ; 471: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 233
	i32 2987532451, ; 472: Xamarin.AndroidX.Security.SecurityCrypto => 0xb21220a3 => 264
	i32 2996846495, ; 473: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 246
	i32 3016983068, ; 474: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 266
	i32 3023353419, ; 475: WindowsBase.dll => 0xb434b64b => 165
	i32 3024354802, ; 476: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 241
	i32 3038032645, ; 477: _Microsoft.Android.Resource.Designer.dll => 0xb514b305 => 326
	i32 3056245963, ; 478: Xamarin.AndroidX.SavedState.SavedState.Ktx => 0xb62a9ccb => 263
	i32 3057625584, ; 479: Xamarin.AndroidX.Navigation.Common => 0xb63fa9f0 => 254
	i32 3059408633, ; 480: Mono.Android.Runtime => 0xb65adef9 => 170
	i32 3059793426, ; 481: System.ComponentModel.Primitives => 0xb660be12 => 16
	i32 3075834255, ; 482: System.Threading.Tasks => 0xb755818f => 144
	i32 3077302341, ; 483: hu/Microsoft.Maui.Controls.resources.dll => 0xb76be845 => 301
	i32 3090735792, ; 484: System.Security.Cryptography.X509Certificates.dll => 0xb838e2b0 => 125
	i32 3099732863, ; 485: System.Security.Claims.dll => 0xb8c22b7f => 118
	i32 3103600923, ; 486: System.Formats.Asn1 => 0xb8fd311b => 38
	i32 3111772706, ; 487: System.Runtime.Serialization => 0xb979e222 => 115
	i32 3121463068, ; 488: System.IO.FileSystem.AccessControl.dll => 0xba0dbf1c => 47
	i32 3124832203, ; 489: System.Threading.Tasks.Extensions => 0xba4127cb => 142
	i32 3132293585, ; 490: System.Security.AccessControl => 0xbab301d1 => 117
	i32 3147165239, ; 491: System.Diagnostics.Tracing.dll => 0xbb95ee37 => 34
	i32 3148237826, ; 492: GoogleGson.dll => 0xbba64c02 => 176
	i32 3159123045, ; 493: System.Reflection.Primitives.dll => 0xbc4c6465 => 95
	i32 3160747431, ; 494: System.IO.MemoryMappedFiles => 0xbc652da7 => 53
	i32 3178803400, ; 495: Xamarin.AndroidX.Navigation.Fragment.dll => 0xbd78b0c8 => 255
	i32 3192346100, ; 496: System.Security.SecureString => 0xbe4755f4 => 129
	i32 3193515020, ; 497: System.Web => 0xbe592c0c => 153
	i32 3204380047, ; 498: System.Data.dll => 0xbefef58f => 24
	i32 3209718065, ; 499: System.Xml.XmlDocument.dll => 0xbf506931 => 161
	i32 3211777861, ; 500: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 232
	i32 3220365878, ; 501: System.Threading => 0xbff2e236 => 148
	i32 3226221578, ; 502: System.Runtime.Handles.dll => 0xc04c3c0a => 104
	i32 3251039220, ; 503: System.Reflection.DispatchProxy.dll => 0xc1c6ebf4 => 89
	i32 3258312781, ; 504: Xamarin.AndroidX.CardView => 0xc235e84d => 220
	i32 3265493905, ; 505: System.Linq.Queryable.dll => 0xc2a37b91 => 60
	i32 3265893370, ; 506: System.Threading.Tasks.Extensions.dll => 0xc2a993fa => 142
	i32 3277815716, ; 507: System.Resources.Writer.dll => 0xc35f7fa4 => 100
	i32 3279906254, ; 508: Microsoft.Win32.Registry.dll => 0xc37f65ce => 5
	i32 3280506390, ; 509: System.ComponentModel.Annotations.dll => 0xc3888e16 => 13
	i32 3286872994, ; 510: SQLite-net.dll => 0xc3e9b3a2 => 200
	i32 3290767353, ; 511: System.Security.Cryptography.Encoding => 0xc4251ff9 => 122
	i32 3299363146, ; 512: System.Text.Encoding => 0xc4a8494a => 135
	i32 3303498502, ; 513: System.Diagnostics.FileVersionInfo => 0xc4e76306 => 28
	i32 3305363605, ; 514: fi\Microsoft.Maui.Controls.resources => 0xc503d895 => 296
	i32 3316684772, ; 515: System.Net.Requests.dll => 0xc5b097e4 => 72
	i32 3317135071, ; 516: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 230
	i32 3317144872, ; 517: System.Data => 0xc5b79d28 => 24
	i32 3340431453, ; 518: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 218
	i32 3345895724, ; 519: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 259
	i32 3346324047, ; 520: Xamarin.AndroidX.Navigation.Runtime => 0xc774da4f => 256
	i32 3357674450, ; 521: ru\Microsoft.Maui.Controls.resources => 0xc8220bd2 => 313
	i32 3358260929, ; 522: System.Text.Json => 0xc82afec1 => 137
	i32 3360279109, ; 523: SQLitePCLRaw.core => 0xc849ca45 => 202
	i32 3362336904, ; 524: Xamarin.AndroidX.Activity.Ktx => 0xc8693088 => 211
	i32 3362522851, ; 525: Xamarin.AndroidX.Core => 0xc86c06e3 => 227
	i32 3366347497, ; 526: Java.Interop => 0xc8a662e9 => 168
	i32 3374999561, ; 527: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 260
	i32 3381016424, ; 528: da\Microsoft.Maui.Controls.resources => 0xc9863768 => 292
	i32 3395150330, ; 529: System.Runtime.CompilerServices.Unsafe.dll => 0xca5de1fa => 101
	i32 3403906625, ; 530: System.Security.Cryptography.OpenSsl.dll => 0xcae37e41 => 123
	i32 3405233483, ; 531: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 231
	i32 3428513518, ; 532: Microsoft.Extensions.DependencyInjection.dll => 0xcc5af6ee => 186
	i32 3429136800, ; 533: System.Xml => 0xcc6479a0 => 163
	i32 3430777524, ; 534: netstandard => 0xcc7d82b4 => 167
	i32 3441283291, ; 535: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 234
	i32 3445260447, ; 536: System.Formats.Tar => 0xcd5a809f => 39
	i32 3452344032, ; 537: Microsoft.Maui.Controls.Compatibility.dll => 0xcdc696e0 => 194
	i32 3463511458, ; 538: hr/Microsoft.Maui.Controls.resources.dll => 0xce70fda2 => 300
	i32 3466904072, ; 539: Microsoft.AspNetCore.SignalR.Client.dll => 0xcea4c208 => 180
	i32 3471940407, ; 540: System.ComponentModel.TypeConverter.dll => 0xcef19b37 => 17
	i32 3476120550, ; 541: Mono.Android => 0xcf3163e6 => 171
	i32 3479583265, ; 542: ru/Microsoft.Maui.Controls.resources.dll => 0xcf663a21 => 313
	i32 3484440000, ; 543: ro\Microsoft.Maui.Controls.resources => 0xcfb055c0 => 312
	i32 3485117614, ; 544: System.Text.Json.dll => 0xcfbaacae => 137
	i32 3486566296, ; 545: System.Transactions => 0xcfd0c798 => 150
	i32 3493954962, ; 546: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 223
	i32 3509114376, ; 547: System.Xml.Linq => 0xd128d608 => 155
	i32 3515174580, ; 548: System.Security.dll => 0xd1854eb4 => 130
	i32 3530912306, ; 549: System.Configuration => 0xd2757232 => 19
	i32 3539954161, ; 550: System.Net.HttpListener => 0xd2ff69f1 => 65
	i32 3560100363, ; 551: System.Threading.Timer => 0xd432d20b => 147
	i32 3570554715, ; 552: System.IO.FileSystem.AccessControl => 0xd4d2575b => 47
	i32 3580758918, ; 553: zh-HK\Microsoft.Maui.Controls.resources => 0xd56e0b86 => 320
	i32 3597029428, ; 554: Xamarin.Android.Glide.GifDecoder.dll => 0xd6665034 => 209
	i32 3598340787, ; 555: System.Net.WebSockets.Client => 0xd67a52b3 => 79
	i32 3608519521, ; 556: System.Linq.dll => 0xd715a361 => 61
	i32 3624195450, ; 557: System.Runtime.InteropServices.RuntimeInformation => 0xd804d57a => 106
	i32 3627220390, ; 558: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 258
	i32 3633644679, ; 559: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 213
	i32 3638274909, ; 560: System.IO.FileSystem.Primitives.dll => 0xd8dbab5d => 49
	i32 3641597786, ; 561: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 244
	i32 3643446276, ; 562: tr\Microsoft.Maui.Controls.resources => 0xd92a9404 => 317
	i32 3643854240, ; 563: Xamarin.AndroidX.Navigation.Fragment => 0xd930cda0 => 255
	i32 3645089577, ; 564: System.ComponentModel.DataAnnotations => 0xd943a729 => 14
	i32 3657292374, ; 565: Microsoft.Extensions.Configuration.Abstractions.dll => 0xd9fdda56 => 185
	i32 3660523487, ; 566: System.Net.NetworkInformation => 0xda2f27df => 68
	i32 3672681054, ; 567: Mono.Android.dll => 0xdae8aa5e => 171
	i32 3676670898, ; 568: Microsoft.Maui.Controls.HotReload.Forms => 0xdb258bb2 => 323
	i32 3682565725, ; 569: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 219
	i32 3684561358, ; 570: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 223
	i32 3691870036, ; 571: Microsoft.AspNetCore.SignalR.Protocols.Json => 0xdc0d7754 => 183
	i32 3697841164, ; 572: zh-Hant/Microsoft.Maui.Controls.resources.dll => 0xdc68940c => 322
	i32 3700866549, ; 573: System.Net.WebProxy.dll => 0xdc96bdf5 => 78
	i32 3706696989, ; 574: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 228
	i32 3716563718, ; 575: System.Runtime.Intrinsics => 0xdd864306 => 108
	i32 3718780102, ; 576: Xamarin.AndroidX.Annotation => 0xdda814c6 => 212
	i32 3724971120, ; 577: Xamarin.AndroidX.Navigation.Common.dll => 0xde068c70 => 254
	i32 3732100267, ; 578: System.Net.NameResolution => 0xde7354ab => 67
	i32 3737834244, ; 579: System.Net.Http.Json.dll => 0xdecad304 => 63
	i32 3748608112, ; 580: System.Diagnostics.DiagnosticSource => 0xdf6f3870 => 27
	i32 3751444290, ; 581: System.Xml.XPath => 0xdf9a7f42 => 160
	i32 3754567612, ; 582: SQLitePCLRaw.provider.e_sqlite3 => 0xdfca27bc => 204
	i32 3786282454, ; 583: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 221
	i32 3787005001, ; 584: Microsoft.AspNetCore.Connections.Abstractions => 0xe1b91c49 => 177
	i32 3792276235, ; 585: System.Collections.NonGeneric => 0xe2098b0b => 10
	i32 3800979733, ; 586: Microsoft.Maui.Controls.Compatibility => 0xe28e5915 => 194
	i32 3802395368, ; 587: System.Collections.Specialized.dll => 0xe2a3f2e8 => 11
	i32 3817368567, ; 588: CommunityToolkit.Maui.dll => 0xe3886bf7 => 173
	i32 3819260425, ; 589: System.Net.WebProxy => 0xe3a54a09 => 78
	i32 3823082795, ; 590: System.Security.Cryptography.dll => 0xe3df9d2b => 126
	i32 3829621856, ; 591: System.Numerics.dll => 0xe4436460 => 83
	i32 3841636137, ; 592: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 0xe4fab729 => 187
	i32 3844307129, ; 593: System.Net.Mail.dll => 0xe52378b9 => 66
	i32 3849253459, ; 594: System.Runtime.InteropServices.dll => 0xe56ef253 => 107
	i32 3870376305, ; 595: System.Net.HttpListener.dll => 0xe6b14171 => 65
	i32 3873536506, ; 596: System.Security.Principal => 0xe6e179fa => 128
	i32 3875112723, ; 597: System.Security.Cryptography.Encoding.dll => 0xe6f98713 => 122
	i32 3876362041, ; 598: SQLite-net => 0xe70c9739 => 200
	i32 3885497537, ; 599: System.Net.WebHeaderCollection.dll => 0xe797fcc1 => 77
	i32 3885922214, ; 600: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 269
	i32 3888767677, ; 601: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 259
	i32 3889960447, ; 602: zh-Hans/Microsoft.Maui.Controls.resources.dll => 0xe7dc15ff => 321
	i32 3896106733, ; 603: System.Collections.Concurrent.dll => 0xe839deed => 8
	i32 3896760992, ; 604: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 227
	i32 3901907137, ; 605: Microsoft.VisualBasic.Core.dll => 0xe89260c1 => 2
	i32 3920810846, ; 606: System.IO.Compression.FileSystem.dll => 0xe9b2d35e => 44
	i32 3921031405, ; 607: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 272
	i32 3928044579, ; 608: System.Xml.ReaderWriter => 0xea213423 => 156
	i32 3930554604, ; 609: System.Security.Principal.dll => 0xea4780ec => 128
	i32 3931092270, ; 610: Xamarin.AndroidX.Navigation.UI => 0xea4fb52e => 257
	i32 3945713374, ; 611: System.Data.DataSetExtensions.dll => 0xeb2ecede => 23
	i32 3953953790, ; 612: System.Text.Encoding.CodePages => 0xebac8bfe => 133
	i32 3955647286, ; 613: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 215
	i32 3959773229, ; 614: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 246
	i32 3980434154, ; 615: th/Microsoft.Maui.Controls.resources.dll => 0xed409aea => 316
	i32 3987592930, ; 616: he/Microsoft.Maui.Controls.resources.dll => 0xedadd6e2 => 298
	i32 4003436829, ; 617: System.Diagnostics.Process.dll => 0xee9f991d => 29
	i32 4015948917, ; 618: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 214
	i32 4023392905, ; 619: System.IO.Pipelines => 0xefd01a89 => 205
	i32 4025784931, ; 620: System.Memory => 0xeff49a63 => 62
	i32 4046471985, ; 621: Microsoft.Maui.Controls.Xaml.dll => 0xf1304331 => 196
	i32 4054681211, ; 622: System.Reflection.Emit.ILGeneration => 0xf1ad867b => 90
	i32 4068434129, ; 623: System.Private.Xml.Linq.dll => 0xf27f60d1 => 87
	i32 4073602200, ; 624: System.Threading.dll => 0xf2ce3c98 => 148
	i32 4094352644, ; 625: Microsoft.Maui.Essentials.dll => 0xf40add04 => 198
	i32 4099507663, ; 626: System.Drawing.dll => 0xf45985cf => 36
	i32 4100113165, ; 627: System.Private.Uri => 0xf462c30d => 86
	i32 4101593132, ; 628: Xamarin.AndroidX.Emoji2 => 0xf479582c => 235
	i32 4102112229, ; 629: pt/Microsoft.Maui.Controls.resources.dll => 0xf48143e5 => 311
	i32 4125707920, ; 630: ms/Microsoft.Maui.Controls.resources.dll => 0xf5e94e90 => 306
	i32 4126470640, ; 631: Microsoft.Extensions.DependencyInjection => 0xf5f4f1f0 => 186
	i32 4127667938, ; 632: System.IO.FileSystem.Watcher => 0xf60736e2 => 50
	i32 4130442656, ; 633: System.AppContext => 0xf6318da0 => 6
	i32 4147896353, ; 634: System.Reflection.Emit.ILGeneration.dll => 0xf73be021 => 90
	i32 4150914736, ; 635: uk\Microsoft.Maui.Controls.resources => 0xf769eeb0 => 318
	i32 4151237749, ; 636: System.Core => 0xf76edc75 => 21
	i32 4159265925, ; 637: System.Xml.XmlSerializer => 0xf7e95c85 => 162
	i32 4161255271, ; 638: System.Reflection.TypeExtensions => 0xf807b767 => 96
	i32 4164802419, ; 639: System.IO.FileSystem.Watcher.dll => 0xf83dd773 => 50
	i32 4181436372, ; 640: System.Runtime.Serialization.Primitives => 0xf93ba7d4 => 113
	i32 4182413190, ; 641: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 251
	i32 4182880526, ; 642: Microsoft.VisualStudio.DesignTools.MobileTapContracts => 0xf951b10e => 324
	i32 4185676441, ; 643: System.Security => 0xf97c5a99 => 130
	i32 4196529839, ; 644: System.Net.WebClient.dll => 0xfa21f6af => 76
	i32 4213026141, ; 645: System.Diagnostics.DiagnosticSource.dll => 0xfb1dad5d => 27
	i32 4256097574, ; 646: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 228
	i32 4258378803, ; 647: Xamarin.AndroidX.Lifecycle.ViewModel.Ktx => 0xfdd1b433 => 250
	i32 4260525087, ; 648: System.Buffers => 0xfdf2741f => 7
	i32 4271975918, ; 649: Microsoft.Maui.Controls.dll => 0xfea12dee => 195
	i32 4274623895, ; 650: CommunityToolkit.Mvvm.dll => 0xfec99597 => 175
	i32 4274976490, ; 651: System.Runtime.Numerics => 0xfecef6ea => 110
	i32 4292120959, ; 652: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 251
	i32 4294763496 ; 653: Xamarin.AndroidX.ExifInterface.dll => 0xfffce3e8 => 237
], align 4

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [654 x i32] [
	i32 68, ; 0
	i32 67, ; 1
	i32 108, ; 2
	i32 247, ; 3
	i32 281, ; 4
	i32 48, ; 5
	i32 80, ; 6
	i32 145, ; 7
	i32 30, ; 8
	i32 322, ; 9
	i32 124, ; 10
	i32 199, ; 11
	i32 102, ; 12
	i32 265, ; 13
	i32 107, ; 14
	i32 265, ; 15
	i32 139, ; 16
	i32 285, ; 17
	i32 77, ; 18
	i32 124, ; 19
	i32 13, ; 20
	i32 221, ; 21
	i32 132, ; 22
	i32 267, ; 23
	i32 151, ; 24
	i32 319, ; 25
	i32 320, ; 26
	i32 18, ; 27
	i32 219, ; 28
	i32 26, ; 29
	i32 178, ; 30
	i32 241, ; 31
	i32 1, ; 32
	i32 59, ; 33
	i32 42, ; 34
	i32 91, ; 35
	i32 224, ; 36
	i32 147, ; 37
	i32 243, ; 38
	i32 240, ; 39
	i32 291, ; 40
	i32 54, ; 41
	i32 69, ; 42
	i32 319, ; 43
	i32 210, ; 44
	i32 83, ; 45
	i32 304, ; 46
	i32 242, ; 47
	i32 203, ; 48
	i32 179, ; 49
	i32 303, ; 50
	i32 131, ; 51
	i32 55, ; 52
	i32 149, ; 53
	i32 74, ; 54
	i32 145, ; 55
	i32 62, ; 56
	i32 146, ; 57
	i32 326, ; 58
	i32 165, ; 59
	i32 315, ; 60
	i32 225, ; 61
	i32 12, ; 62
	i32 238, ; 63
	i32 125, ; 64
	i32 152, ; 65
	i32 182, ; 66
	i32 113, ; 67
	i32 166, ; 68
	i32 164, ; 69
	i32 240, ; 70
	i32 253, ; 71
	i32 84, ; 72
	i32 302, ; 73
	i32 296, ; 74
	i32 193, ; 75
	i32 150, ; 76
	i32 285, ; 77
	i32 60, ; 78
	i32 189, ; 79
	i32 51, ; 80
	i32 103, ; 81
	i32 114, ; 82
	i32 40, ; 83
	i32 278, ; 84
	i32 276, ; 85
	i32 120, ; 86
	i32 310, ; 87
	i32 173, ; 88
	i32 52, ; 89
	i32 44, ; 90
	i32 119, ; 91
	i32 230, ; 92
	i32 308, ; 93
	i32 236, ; 94
	i32 81, ; 95
	i32 136, ; 96
	i32 272, ; 97
	i32 217, ; 98
	i32 8, ; 99
	i32 73, ; 100
	i32 290, ; 101
	i32 155, ; 102
	i32 287, ; 103
	i32 154, ; 104
	i32 92, ; 105
	i32 282, ; 106
	i32 45, ; 107
	i32 305, ; 108
	i32 293, ; 109
	i32 286, ; 110
	i32 109, ; 111
	i32 129, ; 112
	i32 201, ; 113
	i32 25, ; 114
	i32 207, ; 115
	i32 72, ; 116
	i32 55, ; 117
	i32 46, ; 118
	i32 314, ; 119
	i32 192, ; 120
	i32 231, ; 121
	i32 22, ; 122
	i32 245, ; 123
	i32 86, ; 124
	i32 43, ; 125
	i32 160, ; 126
	i32 183, ; 127
	i32 71, ; 128
	i32 258, ; 129
	i32 3, ; 130
	i32 42, ; 131
	i32 63, ; 132
	i32 16, ; 133
	i32 53, ; 134
	i32 317, ; 135
	i32 281, ; 136
	i32 105, ; 137
	i32 286, ; 138
	i32 279, ; 139
	i32 242, ; 140
	i32 34, ; 141
	i32 158, ; 142
	i32 85, ; 143
	i32 32, ; 144
	i32 12, ; 145
	i32 51, ; 146
	i32 56, ; 147
	i32 262, ; 148
	i32 36, ; 149
	i32 187, ; 150
	i32 292, ; 151
	i32 280, ; 152
	i32 215, ; 153
	i32 35, ; 154
	i32 58, ; 155
	i32 249, ; 156
	i32 179, ; 157
	i32 176, ; 158
	i32 17, ; 159
	i32 283, ; 160
	i32 164, ; 161
	i32 305, ; 162
	i32 248, ; 163
	i32 191, ; 164
	i32 275, ; 165
	i32 311, ; 166
	i32 153, ; 167
	i32 271, ; 168
	i32 256, ; 169
	i32 309, ; 170
	i32 217, ; 171
	i32 29, ; 172
	i32 175, ; 173
	i32 52, ; 174
	i32 181, ; 175
	i32 307, ; 176
	i32 276, ; 177
	i32 5, ; 178
	i32 291, ; 179
	i32 266, ; 180
	i32 270, ; 181
	i32 222, ; 182
	i32 287, ; 183
	i32 214, ; 184
	i32 202, ; 185
	i32 233, ; 186
	i32 85, ; 187
	i32 275, ; 188
	i32 61, ; 189
	i32 112, ; 190
	i32 57, ; 191
	i32 321, ; 192
	i32 262, ; 193
	i32 99, ; 194
	i32 19, ; 195
	i32 226, ; 196
	i32 111, ; 197
	i32 101, ; 198
	i32 177, ; 199
	i32 102, ; 200
	i32 289, ; 201
	i32 104, ; 202
	i32 279, ; 203
	i32 71, ; 204
	i32 38, ; 205
	i32 32, ; 206
	i32 103, ; 207
	i32 73, ; 208
	i32 295, ; 209
	i32 9, ; 210
	i32 123, ; 211
	i32 46, ; 212
	i32 216, ; 213
	i32 193, ; 214
	i32 9, ; 215
	i32 43, ; 216
	i32 4, ; 217
	i32 263, ; 218
	i32 299, ; 219
	i32 294, ; 220
	i32 31, ; 221
	i32 138, ; 222
	i32 92, ; 223
	i32 93, ; 224
	i32 314, ; 225
	i32 49, ; 226
	i32 141, ; 227
	i32 112, ; 228
	i32 140, ; 229
	i32 232, ; 230
	i32 115, ; 231
	i32 280, ; 232
	i32 157, ; 233
	i32 323, ; 234
	i32 76, ; 235
	i32 79, ; 236
	i32 252, ; 237
	i32 37, ; 238
	i32 274, ; 239
	i32 174, ; 240
	i32 236, ; 241
	i32 229, ; 242
	i32 64, ; 243
	i32 138, ; 244
	i32 15, ; 245
	i32 116, ; 246
	i32 268, ; 247
	i32 277, ; 248
	i32 224, ; 249
	i32 48, ; 250
	i32 70, ; 251
	i32 80, ; 252
	i32 126, ; 253
	i32 94, ; 254
	i32 121, ; 255
	i32 284, ; 256
	i32 26, ; 257
	i32 203, ; 258
	i32 245, ; 259
	i32 97, ; 260
	i32 28, ; 261
	i32 220, ; 262
	i32 312, ; 263
	i32 290, ; 264
	i32 149, ; 265
	i32 205, ; 266
	i32 169, ; 267
	i32 4, ; 268
	i32 98, ; 269
	i32 33, ; 270
	i32 93, ; 271
	i32 267, ; 272
	i32 189, ; 273
	i32 21, ; 274
	i32 41, ; 275
	i32 170, ; 276
	i32 306, ; 277
	i32 238, ; 278
	i32 298, ; 279
	i32 252, ; 280
	i32 283, ; 281
	i32 277, ; 282
	i32 257, ; 283
	i32 2, ; 284
	i32 134, ; 285
	i32 111, ; 286
	i32 325, ; 287
	i32 190, ; 288
	i32 318, ; 289
	i32 207, ; 290
	i32 315, ; 291
	i32 58, ; 292
	i32 95, ; 293
	i32 297, ; 294
	i32 39, ; 295
	i32 218, ; 296
	i32 325, ; 297
	i32 25, ; 298
	i32 94, ; 299
	i32 89, ; 300
	i32 99, ; 301
	i32 10, ; 302
	i32 87, ; 303
	i32 181, ; 304
	i32 100, ; 305
	i32 264, ; 306
	i32 182, ; 307
	i32 184, ; 308
	i32 284, ; 309
	i32 209, ; 310
	i32 294, ; 311
	i32 7, ; 312
	i32 249, ; 313
	i32 289, ; 314
	i32 206, ; 315
	i32 88, ; 316
	i32 244, ; 317
	i32 154, ; 318
	i32 293, ; 319
	i32 33, ; 320
	i32 116, ; 321
	i32 82, ; 322
	i32 204, ; 323
	i32 20, ; 324
	i32 11, ; 325
	i32 162, ; 326
	i32 3, ; 327
	i32 197, ; 328
	i32 301, ; 329
	i32 192, ; 330
	i32 190, ; 331
	i32 84, ; 332
	i32 288, ; 333
	i32 64, ; 334
	i32 303, ; 335
	i32 271, ; 336
	i32 143, ; 337
	i32 188, ; 338
	i32 253, ; 339
	i32 157, ; 340
	i32 41, ; 341
	i32 117, ; 342
	i32 185, ; 343
	i32 208, ; 344
	i32 297, ; 345
	i32 260, ; 346
	i32 131, ; 347
	i32 75, ; 348
	i32 66, ; 349
	i32 307, ; 350
	i32 172, ; 351
	i32 212, ; 352
	i32 180, ; 353
	i32 143, ; 354
	i32 106, ; 355
	i32 151, ; 356
	i32 70, ; 357
	i32 156, ; 358
	i32 184, ; 359
	i32 121, ; 360
	i32 127, ; 361
	i32 302, ; 362
	i32 152, ; 363
	i32 235, ; 364
	i32 324, ; 365
	i32 0, ; 366
	i32 141, ; 367
	i32 222, ; 368
	i32 299, ; 369
	i32 20, ; 370
	i32 14, ; 371
	i32 135, ; 372
	i32 75, ; 373
	i32 59, ; 374
	i32 201, ; 375
	i32 225, ; 376
	i32 167, ; 377
	i32 168, ; 378
	i32 195, ; 379
	i32 15, ; 380
	i32 74, ; 381
	i32 6, ; 382
	i32 23, ; 383
	i32 247, ; 384
	i32 206, ; 385
	i32 91, ; 386
	i32 300, ; 387
	i32 1, ; 388
	i32 136, ; 389
	i32 248, ; 390
	i32 270, ; 391
	i32 134, ; 392
	i32 69, ; 393
	i32 146, ; 394
	i32 309, ; 395
	i32 288, ; 396
	i32 239, ; 397
	i32 191, ; 398
	i32 88, ; 399
	i32 96, ; 400
	i32 229, ; 401
	i32 234, ; 402
	i32 304, ; 403
	i32 31, ; 404
	i32 45, ; 405
	i32 243, ; 406
	i32 188, ; 407
	i32 208, ; 408
	i32 109, ; 409
	i32 158, ; 410
	i32 35, ; 411
	i32 22, ; 412
	i32 114, ; 413
	i32 57, ; 414
	i32 268, ; 415
	i32 144, ; 416
	i32 118, ; 417
	i32 120, ; 418
	i32 110, ; 419
	i32 210, ; 420
	i32 139, ; 421
	i32 216, ; 422
	i32 54, ; 423
	i32 105, ; 424
	i32 310, ; 425
	i32 196, ; 426
	i32 0, ; 427
	i32 197, ; 428
	i32 133, ; 429
	i32 282, ; 430
	i32 273, ; 431
	i32 261, ; 432
	i32 316, ; 433
	i32 239, ; 434
	i32 199, ; 435
	i32 159, ; 436
	i32 295, ; 437
	i32 226, ; 438
	i32 163, ; 439
	i32 132, ; 440
	i32 261, ; 441
	i32 161, ; 442
	i32 308, ; 443
	i32 250, ; 444
	i32 140, ; 445
	i32 273, ; 446
	i32 269, ; 447
	i32 169, ; 448
	i32 198, ; 449
	i32 174, ; 450
	i32 211, ; 451
	i32 278, ; 452
	i32 40, ; 453
	i32 178, ; 454
	i32 237, ; 455
	i32 81, ; 456
	i32 56, ; 457
	i32 37, ; 458
	i32 97, ; 459
	i32 166, ; 460
	i32 172, ; 461
	i32 274, ; 462
	i32 82, ; 463
	i32 213, ; 464
	i32 98, ; 465
	i32 30, ; 466
	i32 159, ; 467
	i32 18, ; 468
	i32 127, ; 469
	i32 119, ; 470
	i32 233, ; 471
	i32 264, ; 472
	i32 246, ; 473
	i32 266, ; 474
	i32 165, ; 475
	i32 241, ; 476
	i32 326, ; 477
	i32 263, ; 478
	i32 254, ; 479
	i32 170, ; 480
	i32 16, ; 481
	i32 144, ; 482
	i32 301, ; 483
	i32 125, ; 484
	i32 118, ; 485
	i32 38, ; 486
	i32 115, ; 487
	i32 47, ; 488
	i32 142, ; 489
	i32 117, ; 490
	i32 34, ; 491
	i32 176, ; 492
	i32 95, ; 493
	i32 53, ; 494
	i32 255, ; 495
	i32 129, ; 496
	i32 153, ; 497
	i32 24, ; 498
	i32 161, ; 499
	i32 232, ; 500
	i32 148, ; 501
	i32 104, ; 502
	i32 89, ; 503
	i32 220, ; 504
	i32 60, ; 505
	i32 142, ; 506
	i32 100, ; 507
	i32 5, ; 508
	i32 13, ; 509
	i32 200, ; 510
	i32 122, ; 511
	i32 135, ; 512
	i32 28, ; 513
	i32 296, ; 514
	i32 72, ; 515
	i32 230, ; 516
	i32 24, ; 517
	i32 218, ; 518
	i32 259, ; 519
	i32 256, ; 520
	i32 313, ; 521
	i32 137, ; 522
	i32 202, ; 523
	i32 211, ; 524
	i32 227, ; 525
	i32 168, ; 526
	i32 260, ; 527
	i32 292, ; 528
	i32 101, ; 529
	i32 123, ; 530
	i32 231, ; 531
	i32 186, ; 532
	i32 163, ; 533
	i32 167, ; 534
	i32 234, ; 535
	i32 39, ; 536
	i32 194, ; 537
	i32 300, ; 538
	i32 180, ; 539
	i32 17, ; 540
	i32 171, ; 541
	i32 313, ; 542
	i32 312, ; 543
	i32 137, ; 544
	i32 150, ; 545
	i32 223, ; 546
	i32 155, ; 547
	i32 130, ; 548
	i32 19, ; 549
	i32 65, ; 550
	i32 147, ; 551
	i32 47, ; 552
	i32 320, ; 553
	i32 209, ; 554
	i32 79, ; 555
	i32 61, ; 556
	i32 106, ; 557
	i32 258, ; 558
	i32 213, ; 559
	i32 49, ; 560
	i32 244, ; 561
	i32 317, ; 562
	i32 255, ; 563
	i32 14, ; 564
	i32 185, ; 565
	i32 68, ; 566
	i32 171, ; 567
	i32 323, ; 568
	i32 219, ; 569
	i32 223, ; 570
	i32 183, ; 571
	i32 322, ; 572
	i32 78, ; 573
	i32 228, ; 574
	i32 108, ; 575
	i32 212, ; 576
	i32 254, ; 577
	i32 67, ; 578
	i32 63, ; 579
	i32 27, ; 580
	i32 160, ; 581
	i32 204, ; 582
	i32 221, ; 583
	i32 177, ; 584
	i32 10, ; 585
	i32 194, ; 586
	i32 11, ; 587
	i32 173, ; 588
	i32 78, ; 589
	i32 126, ; 590
	i32 83, ; 591
	i32 187, ; 592
	i32 66, ; 593
	i32 107, ; 594
	i32 65, ; 595
	i32 128, ; 596
	i32 122, ; 597
	i32 200, ; 598
	i32 77, ; 599
	i32 269, ; 600
	i32 259, ; 601
	i32 321, ; 602
	i32 8, ; 603
	i32 227, ; 604
	i32 2, ; 605
	i32 44, ; 606
	i32 272, ; 607
	i32 156, ; 608
	i32 128, ; 609
	i32 257, ; 610
	i32 23, ; 611
	i32 133, ; 612
	i32 215, ; 613
	i32 246, ; 614
	i32 316, ; 615
	i32 298, ; 616
	i32 29, ; 617
	i32 214, ; 618
	i32 205, ; 619
	i32 62, ; 620
	i32 196, ; 621
	i32 90, ; 622
	i32 87, ; 623
	i32 148, ; 624
	i32 198, ; 625
	i32 36, ; 626
	i32 86, ; 627
	i32 235, ; 628
	i32 311, ; 629
	i32 306, ; 630
	i32 186, ; 631
	i32 50, ; 632
	i32 6, ; 633
	i32 90, ; 634
	i32 318, ; 635
	i32 21, ; 636
	i32 162, ; 637
	i32 96, ; 638
	i32 50, ; 639
	i32 113, ; 640
	i32 251, ; 641
	i32 324, ; 642
	i32 130, ; 643
	i32 76, ; 644
	i32 27, ; 645
	i32 228, ; 646
	i32 250, ; 647
	i32 7, ; 648
	i32 195, ; 649
	i32 175, ; 650
	i32 110, ; 651
	i32 251, ; 652
	i32 237 ; 653
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 4

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 4

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 4

; Functions

; Function attributes: "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 4, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { noreturn "no-trapping-math"="true" nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+armv7-a,+d32,+dsp,+fp64,+neon,+vfp2,+vfp2sp,+vfp3,+vfp3d16,+vfp3d16sp,+vfp3sp,-aes,-fp-armv8,-fp-armv8d16,-fp-armv8d16sp,-fp-armv8sp,-fp16,-fp16fml,-fullfp16,-sha2,-thumb-mode,-vfp4,-vfp4d16,-vfp4d16sp,-vfp4sp" }

; Metadata
!llvm.module.flags = !{!0, !1, !7}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!"Xamarin.Android remotes/origin/release/8.0.4xx @ a8cd27e430e55df3e3c1e3a43d35c11d9512a2db"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"min_enum_size", i32 4}
