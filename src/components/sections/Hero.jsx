// import React from 'react';
// import { Check, ArrowRight } from 'lucide-react';
// import Button from '../ui/Button';
// import Container from '../ui/Container';
// import { HERO_CONTENT } from '../../data/content';

// // --- استدعاء الصور ---
// import heartImg from '../../assets/Heart_hero.svg';
// import pulseIcon from '../../assets/pulse_icon_Hero.svg';
// import electricIcon from '../../assets/electric_icon_hero.svg';
// import securityIcon from '../../assets/securety_icon_hero.svg';
// import growIcon from '../../assets/Grow_icon_Hero.svg';
// import scrollIcon from '../../assets/Scroll_Down_Hero.svg';
// import nextIcon from '../../assets/icons_link-next.svg';

// const Hero = () => {
//   return (
//     <section className="relative bg-neutral-50 pt-12 pb-24 overflow-hidden min-h-[90vh] flex items-center">
//       <Container>
//         <div className="lg:grid lg:grid-cols-2 lg:gap-16 items-center">
          
//           {/* === LEFT SIDE: Text Content === */}
//           <div className="mb-16 lg:mb-0 z-10 relative">
//             <h1 className="text-6xl md:text-7xl font-bold text-main mb-6 font-display tracking-tight leading-tight">
//               {HERO_CONTENT.title}<span className="text-primary">{HERO_CONTENT.highlight}</span>
//             </h1>
//             <p className="text-lg text-muted mb-8 leading-relaxed max-w-lg font-sans">
//               {HERO_CONTENT.subtitle}
//             </p>
//             <div className="space-y-4 mb-10 font-sans">
//               {HERO_CONTENT.features.map((item, i) => (
//                 <div key={i} className="flex items-center gap-3">
//                   <div className="flex-shrink-0 w-5 h-5 rounded-full bg-teal-50 flex items-center justify-center">
//                     <Check className="w-3 h-3 text-teal-500" strokeWidth={3} />
//                   </div>
//                   <span className="text-[16px] text-main font-medium">{item}</span>
//                 </div>
//               ))}
//             </div>
//             <Button className="w-[160px] h-[52px] group">
//               Get Started
//               <img src={nextIcon} alt="arrow" className="ml-2 w-3 h-3 transition-transform group-hover:translate-x-1" />
//             </Button>
//           </div>

//           {/* === RIGHT SIDE: Heart & Icons === */}
//           <div className="relative h-[600px] w-full flex justify-center items-center">
            
//             {/* 1. The Heart (ينبض) */}
//             <div className="relative z-10 w-[480px]">
//                <img 
//                  src={heartImg} 
//                  alt="Heart Model" 
//                  className="w-full h-auto drop-shadow-2xl animate-heartbeat" 
//                />
//             </div>

//             {/* --- الأيقونات (تنبض) --- */}
            
//             {/* أ. Green Pulse Icon */}
//             <img 
//               src={pulseIcon} 
//               alt="Pulse" 
//               className="absolute top-[20%] right-[5%] w-16 h-16 animate-heartbeat z-0 opacity-80"
//             />
            
//             {/* ب. Electric Icon */}
//             <img 
//               src={electricIcon} 
//               alt="Electric" 
//               className="absolute bottom-[25%] left-[10%] w-10 h-14 animate-heartbeat z-0"
//             />

//             {/* ج. Security Icon (التعديل: نقلناها فوق كارت الـ Risk على اليمين) */}
//             <img 
//               src={securityIcon} 
//               alt="Security" 
//               // bottom-[40%] عشان تبقى فوق كارت Risk اللي هو عند 20%
//               className="absolute bottom-[40%] right-[2%] w-12 h-12 animate-heartbeat z-0"
//             />


//             {/* --- الكروت (Divs) - (التعديل: شلنا animate-float وبقوا ثابتين) --- */}

//             {/* 1. Floating Card: BPM */}
//             <div className="absolute top-[15%] left-0 md:left-[0%] bg-white p-3 px-5 rounded-2xl shadow-[0px_4px_20px_rgba(0,0,0,0.08)] flex items-center gap-3 z-20">
//               <div className="w-2.5 h-2.5 bg-emerald-500 rounded-full animate-pulse"></div>
//               <div>
//                 <p className="font-bold text-main text-sm font-sans">BPM: 72</p>
//                 <p className="text-[10px] text-muted font-sans">Normal Range</p>
//               </div>
//             </div>

//             {/* 2. Floating Card: Risk */}
//             <div className="absolute bottom-[20%] right-0 md:right-[0%] bg-white p-3 px-5 rounded-2xl shadow-[0px_4px_20px_rgba(0,0,0,0.08)] flex items-center gap-3 z-20">
//               <div>
//                  <img src={growIcon} alt="Grow" className="w-5 h-5" />
//               </div>
//               <div>
//                 <p className="font-bold text-main text-sm font-sans">Risk: Low</p>
//                 <p className="text-[10px] text-muted font-sans">AI Assessment</p>
//               </div>
//             </div>

//           </div>
//         </div>

//         {/* === BOTTOM: Scroll Indicator === */}
//         <div className="absolute bottom-10 left-1/2 transform -translate-x-1/2 flex flex-col items-center gap-2 animate-bounce-slow cursor-pointer opacity-70 hover:opacity-100 transition-opacity">
//           <span className="text-sm text-muted font-medium font-sans">Scroll to explore</span>
//           <img src={scrollIcon} alt="Scroll Down" className="w-4 h-4" />
//         </div>

//       </Container>
//     </section>
//   );
// };

// export default Hero;


// import React from 'react';
// // ملحوظة: شيلنا Check من هنا لأننا استبدلناها بالصورة خلاص
// import { ArrowRight } from 'lucide-react';
// import Button from '../ui/Button';
// import Container from '../ui/Container';
// import { HERO_CONTENT } from '../../data/content';

// // --- استدعاء الصور ---
// import heartImg from '../../assets/Heart_hero.svg';
// import pulseIcon from '../../assets/pulse_icon_Hero.svg';
// import electricIcon from '../../assets/electric_icon_hero.svg';
// import securityIcon from '../../assets/securety_icon_hero.svg';
// import growIcon from '../../assets/Grow_icon_Hero.svg';
// import scrollIcon from '../../assets/Scroll_Down_Hero.svg';
// import nextIcon from '../../assets/icons_link-next.svg';
// // --- التعديل هنا: استدعاء صورة علامة الصح الجديدة ---
// // تأكدي إن اسم الملف ومساره صحيح (أنا افترضت إنه RightHero.svg)
// import rightImg from '../../assets/RightHero.svg';

// const Hero = () => {
//   return (
//     <section className="relative bg-neutral-50 pt-12 pb-24 overflow-hidden min-h-[90vh] flex items-center">
//       <Container>
//         <div className="lg:grid lg:grid-cols-2 lg:gap-16 items-center">
          
//           {/* === LEFT SIDE: Text Content === */}
//           <div className="mb-16 lg:mb-0 z-10 relative">
//             <h1 className="text-6xl md:text-7xl font-bold text-main mb-6 font-display tracking-tight leading-tight">
//               {HERO_CONTENT.title}<span className="text-primary">{HERO_CONTENT.highlight}</span>
//             </h1>
//             <p className="text-lg text-muted mb-8 leading-relaxed max-w-lg font-sans">
//               {HERO_CONTENT.subtitle}
//             </p>
            
//             <div className="space-y-4 mb-10 font-sans">
//               {HERO_CONTENT.features.map((item, i) => (
//                 <div key={i} className="flex items-center gap-3">
//                   {/* --- التعديل هنا --- */}
//                   {/* استبدلنا الـ div الملون القديم بصورة الـ RightHero */}
//                   <img 
//                     src={rightImg} 
//                     alt="check" 
//                     className="flex-shrink-0 w-5 h-5" // حافظنا على نفس المقاس
//                   />
//                   <span className="text-[16px] text-main font-medium">{item}</span>
//                 </div>
//               ))}
//             </div>
            
//             <Button className="w-[160px] h-[52px] group">
//               Get Started
//               <img src={nextIcon} alt="arrow" className="ml-2 w-3 h-3 transition-transform group-hover:translate-x-1" />
//             </Button>
//           </div>

//           {/* === RIGHT SIDE: Heart & Icons (زي ما هو بدون تغيير) === */}
//           <div className="relative h-[600px] w-full flex justify-center items-center">
//             <div className="relative z-10 w-[480px]">
//                <img src={heartImg} alt="Heart Model" className="w-full h-auto drop-shadow-2xl animate-heartbeat" />
//             </div>
//             <img src={pulseIcon} alt="Pulse" className="absolute top-[20%] right-[5%] w-16 h-16 animate-heartbeat z-0 opacity-80" />
//             <img src={electricIcon} alt="Electric" className="absolute bottom-[25%] left-[10%] w-10 h-14 animate-heartbeat z-0" />
//             <img src={securityIcon} alt="Security" className="absolute bottom-[40%] right-[2%] w-12 h-12 animate-heartbeat z-0" />

//             {/* Cards */}
//             <div className="absolute top-[15%] left-0 md:left-[0%] bg-white p-3 px-5 rounded-2xl shadow-[0px_4px_20px_rgba(0,0,0,0.08)] flex items-center gap-3 z-20">
//               <div className="w-2.5 h-2.5 bg-emerald-500 rounded-full animate-pulse"></div>
//               <div>
//                 <p className="font-bold text-main text-sm font-sans">BPM: 72</p>
//                 <p className="text-[10px] text-muted font-sans">Normal Range</p>
//               </div>
//             </div>
//             <div className="absolute bottom-[20%] right-0 md:right-[0%] bg-white p-3 px-5 rounded-2xl shadow-[0px_4px_20px_rgba(0,0,0,0.08)] flex items-center gap-3 z-20">
//               <div><img src={growIcon} alt="Grow" className="w-5 h-5" /></div>
//               <div>
//                 <p className="font-bold text-main text-sm font-sans">Risk: Low</p>
//                 <p className="text-[10px] text-muted font-sans">AI Assessment</p>
//               </div>
//             </div>
//           </div>
//         </div>

//         {/* === BOTTOM: Scroll Indicator === */}
//         <div className="absolute bottom-10 left-1/2 transform -translate-x-1/2 flex flex-col items-center gap-2 animate-bounce-slow cursor-pointer opacity-70 hover:opacity-100 transition-opacity">
//           <span className="text-sm text-muted font-medium font-sans">Scroll to explore</span>
//           <img src={scrollIcon} alt="Scroll Down" className="w-4 h-4" />
//         </div>

//       </Container>
//     </section>
//   );
// };
import React from 'react';
import Button from '../ui/Button';
import { HERO_CONTENT } from '../../data/content';

import heartImg from '../../assets/Heart_hero.svg';
import pulseIcon from '../../assets/pulse_icon_Hero.svg';
import electricIcon from '../../assets/electric_icon_hero.svg';
import securityIcon from '../../assets/securety_icon_hero.svg';
import growIcon from '../../assets/Grow_icon_Hero.svg';
import scrollIcon from '../../assets/Scroll_Down_Hero.svg';
import rightImg from '../../assets/RightHero.svg';
import nextIcon from '../../assets/icons_link-next.svg';

const Hero = () => {
  return (
    <section id="home" className="relative bg-[#F9FAFB] pt-[80px] pb-40 overflow-hidden min-h-[calc(100vh-82px)] flex items-center justify-center">
      
      <div className="w-full max-w-[1440px] mx-auto px-6 md:px-[80px]">
        <div className="lg:grid lg:grid-cols-2 items-center gap-16">
          
           <div 
             className="z-10 relative flex flex-col justify-center"
             data-aos="fade-right" 
             data-aos-delay="200"
           >
            <h1 className="text-6xl md:text-[64px] font-bold text-main mb-6 font-display tracking-tight leading-[1.1]">
              {HERO_CONTENT.title}<span className="text-primary text-7xl md:text-[72px]">{HERO_CONTENT.highlight}</span>
            </h1>
            
            <p className="text-xl text-muted font-normal font-sans mb-8 leading-[1.6] max-w-[600px]">
              {HERO_CONTENT.subtitle}
            </p>
            
            <div className="space-y-4 mb-12 font-sans">
              {HERO_CONTENT.features.map((item, i) => (
                <div key={i} className="flex items-center gap-4">
                  <img src={rightImg} alt="check" className="flex-shrink-0 w-5 h-5" />
                  <span className="text-xl text-main font-medium">{item}</span>
                </div>
              ))}
            </div>

          <Button className="w-[176px] h-[48px] !text-[15px] !rounded-[10px] bg-[#2563EB] hover:bg-[#1d4ed8] shadow-lg group flex items-center justify-center gap-2">
            Get Started
            <img src={nextIcon} alt="arrow" className="w-[14px] h-[12px] text-white transition-transform group-hover:translate-x-1" />
          </Button>
          </div>

          <div 
            className="relative h-[500px] w-full flex justify-center items-center"
            data-aos="fade-left" 
            data-aos-delay="400"
          >
            
            <div className="relative z-10 w-[461px] h-[461px]">
               <img 
                 src={heartImg} 
                 alt="Heart Model" 
                 className="w-full h-auto drop-shadow-2xl animate-heartbeat" 
               />
            </div>

            <img 
              src={pulseIcon} 
              alt="Pulse" 
              className="absolute top-[20%] right-[5%] w-[100px] h-[100px] animate-heartbeat z-0 opacity-80"
            />
            
            <img 
              src={electricIcon} 
              alt="Electric" 
              className="absolute bottom-[30%] left-[10%] w-[66px] h-[66px] animate-heartbeat z-0"
            />

            <div className="absolute top-[25%] left-[-2%] bg-white w-[128px] h-[64px] p-2 rounded-lg shadow-[0px_4px_12px_0px_rgba(0,0,0,0.12)] flex flex-col justify-center items-center z-20">
              <div className="flex items-center gap-1">
                <div className="w-2.5 h-2.5 bg-emerald-500 rounded-full animate-pulse"></div>
                <p className="text-main text-base font-medium font-sans">BPM: 72</p>
              </div>
              <p className="text-muted text-xs font-normal font-sans">Normal Range</p>
            </div>

            <div className="absolute bottom-[20%] right-[-2%] bg-white w-[128px] h-[64px] p-2 rounded-lg shadow-[0px_4px_12px_0px_rgba(0,0,0,0.12)] flex flex-col justify-center items-center z-20">
               <div className="relative w-full flex justify-center mb-1">
                   <img 
                     src={securityIcon} 
                     alt="Shield" 
                     className="absolute -top-8 -right-2 w-[40px] h-[40px] animate-heartbeat" 
                   />
               </div>
               <div className="flex items-center gap-1.5">
                 <img src={growIcon} alt="Grow" className="w-4 h-4" />
                 <p className="text-main text-base font-medium font-sans">Risk: Low</p>
               </div>
               <p className="text-muted text-xs font-normal font-sans text-center">AI Assessment</p>
            </div>

          </div>
        </div>

        <div className="absolute bottom-8 left-1/2 transform -translate-x-1/2 flex flex-col items-center gap-2 cursor-pointer hover:opacity-100 transition-opacity opacity-60 animate-bounce">
          <span className="text-muted text-[13px] font-medium font-sans tracking-wide">Scroll to explore</span>
          <img src={scrollIcon} alt="Scroll Down" className="w-4 h-4" />
        </div>

      </div>
    </section>
  );
};

export default Hero;