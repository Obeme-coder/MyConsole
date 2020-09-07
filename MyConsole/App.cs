﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace MyConsole
{
    public static class App
    {
        public static Stack<IWindow> path = new Stack<IWindow>();
        public static IWindow SettingsMenu;
        public static Thread th;
        public static Mutex m = new Mutex();
        public static Dictionary<string, dynamic> values = new Dictionary<string, dynamic>();
        public static Dictionary<string, dynamic> settings = new Dictionary<string, dynamic>()
        {
            { "update frequency", 23f },
            { "fs", 23f },
            { "fsdf", true },
        };
        public static void Forward(IWindow window)
        {
            Thread t = new Thread(_Forward);
            t.Start(window);
            t.Join();
        }
        public static void _Forward(object window)
        {
            m.WaitOne();
            path.Peek().Exit();
            th.Abort();
            path.Push((IWindow)window);
            th = new Thread(path.Peek().Start);
            th.Start();
            m.ReleaseMutex();
        }
        public static void Change(IWindow window)
        {
            Thread t = new Thread(_Change);
            t.Start(window);
            t.Join();
        }
        public static void _Change(object window)
        {
            m.WaitOne();
            path.Peek().Exit();
            path.Pop();
            th.Abort();

            path.Push((IWindow)window);
            th = new Thread(path.Peek().Start);
            th.Start();
            m.ReleaseMutex();
        }
        public static void Back()
        {
            Thread t = new Thread(_Back);
            t.Start();
            t.Join();
        }
        public static void _Back()
        {
            m.WaitOne();
            path.Peek().Exit();
            path.Pop();
            th.Abort();
            if (path.Count != 0)
            {
                th = new Thread(path.Peek().Start);
                th.Start();
            }
            else
            {
                Exit();
            }
            m.ReleaseMutex();
        }
        static void Exit()
        {
            new Thread(() => Console.Beep(300, 1000)).Start(); ;
            Console.Clear();
            Console.WriteLine(exit);
            Thread.Sleep(700);
            Console.Clear();
            Console.WriteLine(skeleton);
            Thread.Sleep(50);
            Console.Clear();
            Console.WriteLine(exit);
            Thread.Sleep(50);
            Process.GetCurrentProcess().Kill();
        }
        public static void Start(IWindow mainMenu)
        {
            path.Push(mainMenu);
            th = new Thread(path.Peek().Start);
            th.Start();
        }
        static string exit = @"
 /$$$$$$$$           /$$   /$$    
| $$_____/          |__/  | $$    
| $$       /$$   /$$ /$$ /$$$$$$  
| $$$$$   |  $$ /$$/| $$|_  $$_/  
| $$__/    \  $$$$/ | $$  | $$    
| $$        >$$  $$ | $$  | $$ /$$
| $$$$$$$$ /$$/\  $$| $$  |  $$$$/
|________/|__/  \__/|__/   \___/  
";
        static string skeleton = @"
                                       :xx++::
                                  .!!X!~""?!`~!~!. :-:.
                                .!!!H"":.~ ::+!~~!!!~ `%X.
                                ~~!M!!>!!X?!!!!!!!!!!...!~.
                              <!:!MM!~:XM!!!!!!.:!..~ !.  `<
                         :~ .:<~!!M!XXHM!!!X!XXHtMMHHHX!  ~ ~
                ~~~~<' ~!!!:!!!!!XM!!M!!!XHMMMRMSXXX!!!!!!:  <`
                  `<  <::!!!!!X!X?M!!M!!XMMMMXXMMMM??!!!!!?!:~<
               : '~~~<!!!XMMH!!XMXMXHHXXXXM!!!!MMMMSXXXX!!!!!!!~
            :    ::`~!!!MMMMXXXtMMMMMMMMMMMHX!!!!!!HMMMMMX!!!!!: ~
               '~:~!!!!!MMMMMMMMMMMMMMMMMMMMMMXXX!!!M??MMMM!!X!!i:
               <~<!!!!!XMMMMMMMMMMMM8M8MMMMM8MMMMMXX!!!!!!!!X!?t?!:
               ~:~~!!!!?MMMMMM@M@RMRRR$@@MMRMRMMMMMMXSX!!!XMMMX<?X!
             :XX <!!XHMMMM88MM88BR$M$$$$8@8RN88MMMMMMMMHXX?MMMMMX!!!
           .:X! <XMSM8M@@$$$$$$$$$$$$$$$$$$$B8R$8MMMMMMMMMMMMMMMMX!X
          :!?! !?XMMMMM8$$$$8$$$$$$$$$$$$$$BBR$$MMM@MMMMMMMMMMMMMM!!X
        ~<!!~ <!!XMMMB$$$$$$$$$$$$$$$$$$$$$$$$MMR$8MR$MMMMMMMMMMMMM!?!:
        :~~~ !:X!XMM8$$$$$$$$$$$$$$$$$$$$$$$RR$$MMMMR8NMMMMMMMMMMMMM<!`-
    ~:<!:~`~':!:HMM8N$$$$$$$$$$$$$$$$$$$$$$$$$8MRMM8R$MRMMMMMMMMRMMMX!
  !X!``~~   :~XM?SMM$B$$$$$$$$$$$$$$$$$$$$$$BR$$MMM$@R$M$MMMMMM$MMMMX?L
 X~.      : `!!!MM#$RR$$$$$$$$$$$$$$$$$R$$$$$R$M$MMRRRM8MMMMMMM$$MMMM!?:
 ! ~ <~  !! !!~`` :!!MR$$$$$$$$$$RMM!?!??RR?#R8$M$MMMRM$RMMMM8MM$MMM!M!:>
: ' >!~ '!!  !   .!XMM8$$$$$@$$$R888HMM!!XXHWX$8$RM$MR5$8MMMMR$$@MMM!!!< ~
!  ' !  ~!! :!:XXHXMMMR$$$$$$$$$$$$$$$$8$$$$8$$$MMR$M$$$MMMMMM$$$MMM!!!!
 ~<!!!  !!! !!HMMMMMMMM$$$$$$$$$$$$$$$$$$$$$$$$$$MMM$M$$MM8MMMR$$MMXX!!!!/:`
  ~!!!  !!! !XMMMMMMMMMMR$$$$$$$$$$$$R$RRR$$$$$$$MMMM$RM$MM8MM$$$M8MMMX!!!!:
  !~ ~  !!~ XMMM%!!!XMMX?M$$$$$$$$B$MMSXXXH?MR$$8MMMM$$@$8$M$B$$$$B$MMMX!!!!
  ~!    !! 'XMM?~~!!!MMMX!M$$$$$$MRMMM?!%MMMH!R$MMMMMM$$$MM$8$$$$$$MR@M!!!!!
  <>    !!  !Mf x@#""~!t?M~!$$$$$RMMM?Xb@!~`??MS$M@MMM@RMRMMM$$$$$$RMMMMM!!!!
  !    '!~ <!!:!?M   !@!M<XM$$R5M$8MMM$! -XXXMMRMBMMM$RMMM@$R$BR$MMMMX??!X!!
  !    '!  !!X!!!?::xH!HM:MM$RM8M$RHMMMX...XMMMMM$RMMRRMMMMMMM8MMMMMMMMX!!X!
  !     ~  !!?:::!!!MXMR~!MMMRMM8MMMMMS!!M?XXMMMMM$$M$M$RMMMM8$RMMMMMMMM%X!!
  ~     ~  !~~X!!XHMMM?~ XM$MMMMRMMMMMM@MMMMMMMMMM$8@MMMMMMMMRMMMMM?!MMM%HX!
           !!!!XSMMXXMM .MMMMMMMM$$$BB8MMM@MMMMMMMR$RMMMMMMMMMMMMMMMXX!?H!XX
           XHXMMMMMMMM!.XMMMMMMMMMR$$$8M$$$$$M@88MMMMMMMMMMMMMMM!XMMMXX!!!XM
      ~   <!MMMMMMMMRM:XMMMMMMMMMM8R$$$$$$$$$$$$$$$NMMMMMMMM?!MM!M8MXX!!/t!M
      '   ~HMMMMMMMMM~!MM8@8MMM!MM$$8$$$$$$$$$$$$$$8MMMMMMM!!XMMMM$8MR!MX!MM
          'MMMMMMMMMM'MM$$$$$MMXMXM$$$$$$$$$$$$$$$$RMMMMMMM!!MMM$$$$MMMMM<!M
          'MMMMMMMMM!'MM$$$$$RMMMMMM$$$$$$$$$$$$$$$MMM!MMMX!!MM$$$$$M$$M$M!M
           !MMMMMM$M! !MR$$$RMM8$8MXM8$$$$$$$$$$$$NMMM!MMM!!!?MRR$$RXM$$MR!M
           !M?XMM$$M.< !MMMMMMSUSRMXM$8R$$$$$$$$$$#$MM!MMM!X!t8$M$MMMHMRMMX$
    ,-,   '!!!MM$RMSMX:.?!XMHRR$RM88$$$8M$$$$$R$$$$8MM!MMXMH!M$$RMMMMRNMMX!$
   -'`    '!!!MMMMMMMMMM8$RMM8MBMRRMR8RMMM$$$$8$8$$$MMXMMMMM!MR$MM!M?MMMMMM$
          'XX!MMMMMMM@RMM$MM@$$BM$$$M8MMMMR$$$$@$$$$MM!MMMMXX$MRM!XH!!??XMMM
          `!!!M?MHMMM$RMMMR@$$$$MR@MMMM8MMMM$$$$$$$WMM!MMMM!M$RMM!!.MM!%M?~!
           !!!!!!MMMMBMM$$RRMMMR8MMMMMRMMMMM8$$$$$$$MM?MMMM!f#RM~    `~!!!~!
           ~!!HX!!~!?MM?MMM??MM?MMMMMMMMMRMMMM$$$$$MMM!MMMM!!
           '!!!MX!:`~~`~~!~~!!!!XM!!!?!?MMMM8$$$$$MMMMXMMM!!
            !!~M@MX.. <!!X!!!!XHMHX!!``!XMMMB$MM$$B$M!MMM!!
            !!!?MRMM!:!XHMHMMMMMMMM!  X!SMMX$$MM$$$RMXMMM~
             !M!MMMM>!XMMMMMMMMXMM!!:!MM$MMMBRM$$$$8MMMM~
             `?H!M$R>'MMMM?MMM!MM6!X!XM$$$MM$MM$$$$MX$f
              `MXM$8X MMMMMMM!!MM!!!!XM$$$MM$MM$$$RX@""
               ~M?$MM !MMMMXM!!MM!!!XMMM$$$8$XM$$RM!`
                !XMMM !MMMMXX!XM!!!HMMMM$$$$RH$$M!~
                'M?MM `?MMXMM!XM!XMMMMM$$$$$RM$$#
                 `>MMk ~MMHM!XM!XMMM$$$$$$BRM$M""
                  ~`?M. !M?MXM!X$$@M$$$$$$RMM#
                    `!M  !!MM!X8$$$RM$$$$MM#`
                      !% `~~~X8$$$$8M$$RR#`
                       !!x:xH$$$$$$$R$R*`
                        ~!?MMMMRRRM@M#`       
                         `~???MMM?M""`
                             ``~~
";
    }
}
