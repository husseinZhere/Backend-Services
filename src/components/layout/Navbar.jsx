// import React, { useState } from 'react';
// import { Menu, X } from 'lucide-react';
// import { useToggle } from '../../hooks/useToggle';
// import { NAV_LINKS, BRAND } from '../../data/content';
// import Container from '../ui/Container';
// import Button from '../ui/Button';

// // Images
// import logoImg from '../../assets/logo.svg';
// import modeIcon from '../../assets/mode.svg';
// import nextIcon from '../../assets/icons_link-next.svg';

// const Navbar = () => {
//   const [isOpen, toggleOpen] = useToggle(false);
//   const [activeLink, setActiveLink] = useState('Home');

//   return (
//     <nav className="bg-white h-[82px] border-b border-gray-100 sticky top-0 z-50 flex items-center font-sans">
//       <Container>
//         <div className="flex justify-between items-center w-full">
          
//           {/* === GROUP 1: LEFT SIDE (Logo + Links) === */}
//           {/* التعديل 1: قللنا الـ gap لـ 50px عشان نقرب اللينكات من اللوجو شوية 
//               فيبعدوا عن العربية
//           */}
//           <div className="flex items-center gap-[130px]">
            
//             {/* Logo */}
//             <div 
//               className="flex-shrink-0 flex items-center gap-1 cursor-pointer" 
//               onClick={() => setActiveLink('Home')}
//             >
//               <img src={logoImg} alt="PulseX" className="h-8 w-auto object-contain" />
//               <span className="text-main text-[18px] font-bold font-['Poppins'] tracking-tight">
//                 {BRAND.name}
//               </span>
//             </div>
            
//             {/* Links */}
//             <div className="hidden md:flex items-center gap-[40px]">
//               {NAV_LINKS.map((link) => {
//                 const isActive = activeLink === link.label;
//                 return (
//                   <a 
//                     key={link.label} 
//                     href={link.href}
//                     onClick={(e) => { e.preventDefault(); setActiveLink(link.label); }}
//                     className="relative flex flex-col items-center h-[30px] justify-center group"
//                   >
//                     <span className={`
//                       text-[15px] font-medium transition-colors duration-200
//                       ${isActive ? 'text-primary' : 'text-muted group-hover:text-primary'}
//                     `}>
//                       {link.label}
//                     </span>
                    
//                     <div className={`
//                       absolute bottom-0 h-[2px] rounded-full bg-primary transition-all duration-300 ease-out
//                       ${isActive ? 'w-full opacity-100' : 'w-0 opacity-0 group-hover:w-1/2 group-hover:opacity-30'}
//                     `}></div>
//                   </a>
//                 );
//               })}
//             </div>
//           </div>

//           {/* === GROUP 2: RIGHT SIDE (Utilities + Buttons) === */}
//           {/* التعديل 2: ضفنا pl-10 عشان نعمل مسافة أمان إجبارية قبل كلمة العربية */}
//           <div className="hidden md:flex items-center gap-[40px] pl-10">
            
//             {/* Language & Mode */}
//             <div className="flex items-center gap-6">
//               <button className="text-muted hover:text-primary transition-colors text-[14px] font-medium font-['Inter']">
//                 العربية
//               </button>
//               <button className="hover:opacity-70 transition-opacity p-1">
//                 <img src={modeIcon} alt="Dark Mode" className="w-[20px] h-[20px]" />
//               </button>
//             </div>

//             {/* Buttons Group */}
//             <div className="flex items-center gap-3">
//               <Button variant="outline" className="min-w-[90px]">
//                 Log in
//               </Button>
              
//               <Button variant="primary" className="min-w-[140px] group">
//                 Get Started
//                 <img src={nextIcon} alt="arrow" className="ml-2 w-3 h-3 transition-transform group-hover:translate-x-1" />
//               </Button>
//             </div>
//           </div>

//           {/* Mobile Toggle */}
//           <div className="flex items-center md:hidden">
//             <button onClick={toggleOpen} className="p-2 text-muted hover:text-primary">
//               {isOpen ? <X /> : <Menu />}
//             </button>
//           </div>
//         </div>
//       </Container>

//       {/* Mobile Menu */}
//       {isOpen && (
//         <div className="absolute top-[82px] left-0 w-full bg-white border-t shadow-lg md:hidden p-4 flex flex-col gap-4 animate-fade-in">
//            {NAV_LINKS.map((link) => (
//              <a 
//                key={link.label} 
//                href="#" 
//                className={`font-medium py-2 border-b border-gray-50 transition-colors
//                ${activeLink === link.label ? 'text-primary' : 'text-muted hover:text-primary'}`}
//                onClick={() => setActiveLink(link.label)}
//              >
//                {link.label}
//              </a>
//            ))}
//            <div className="flex justify-between pt-2 items-center">
//              <span>العربية</span>
//              <img src={modeIcon} alt="Dark Mode" className="w-6 h-6" />
//            </div>
//            <div className="flex flex-col gap-3 mt-2">
//              <Button variant="outline" className="w-full justify-center">Log in</Button>
//              <Button variant="primary" className="w-full justify-center">
//                 Get Started
//              </Button>
//            </div>
//         </div>
//       )}
//     </nav>
//   );
// };

// export default Navbar;



// import React, { useState } from 'react';
// import { Menu, X } from 'lucide-react';
// import { useToggle } from '../../hooks/useToggle';
// import { NAV_LINKS, BRAND } from '../../data/content';
// // شيلنا Container العادي وهنعمل واحد مخصص هنا عشان نظبط الـ 80px
// import Button from '../ui/Button';

// // Images
// import logoImg from '../../assets/logo.svg';
// import modeIcon from '../../assets/mode.svg';
// import nextIcon from '../../assets/icons_link-next.svg';

// const Navbar = () => {
//   const [isOpen, toggleOpen] = useToggle(false);
//   const [activeLink, setActiveLink] = useState('Home');

//   return (
//     // 1. Height: 82px
//     <nav className="bg-white h-[82px] shadow-[0px_4px_10px_rgba(0,0,0,0.05)] sticky top-0 z-50 flex items-center font-sans">
//       {/* 2. Container Width: 1440px | Padding Left: 80px */}
//       <div className="w-full max-w-[1440px] mx-auto px-6 md:px-[80px]">
//         <div className="flex justify-between items-center w-full">
          
//           {/* === LEFT SIDE === */}
//           <div className="flex items-center">
//             {/* Logo */}
//             <div 
//               className="flex-shrink-0 flex items-center gap-3 cursor-pointer mr-[100px]" // مسافة تقديرية بين اللوجو واللينكات للحفاظ على الشكل
//               onClick={() => setActiveLink('Home')}
//             >
//               {/* Logo Size: 32x27 from Figma */}
//               <img src={logoImg} alt="PulseX" className="w-[32px] h-[27px] object-contain" />
//               <span className="text-main text-[18px] font-bold font-['Poppins'] tracking-tight">
//                 {BRAND.name}
//               </span>
//             </div>
            
//             {/* Links: Gap 48px (From Component 36) */}
//             <div className="hidden md:flex items-center gap-[48px]">
//               {NAV_LINKS.map((link) => {
//                 const isActive = activeLink === link.label;
//                 return (
//                   <a 
//                     key={link.label} 
//                     href={link.href}
//                     onClick={(e) => { e.preventDefault(); setActiveLink(link.label); }}
//                     className="relative flex flex-col items-center h-[26px] justify-between group"
//                   >
//                     <span className={`
//                       text-[14px] font-medium transition-colors duration-200
//                       ${isActive ? 'text-primary' : 'text-muted group-hover:text-primary'}
//                     `}>
//                       {link.label}
//                     </span>
                    
//                     <div className={`
//                       h-[2px] rounded-full bg-primary transition-all duration-300 ease-out
//                       ${isActive ? 'w-full opacity-100' : 'w-0 opacity-0 group-hover:w-1/2 group-hover:opacity-30'}
//                     `}></div>
//                   </a>
//                 );
//               })}
//             </div>
//           </div>

//           {/* === RIGHT SIDE === */}
//           {/* Gap: 56px (Between Frame 7170 and Frame 7169) */}
//           <div className="hidden md:flex items-center gap-[56px]">
            
//             {/* Language & Mode */}
//             <div className="flex items-center gap-6">
//               <button className="text-muted hover:text-primary transition-colors text-[14px] font-medium font-['Inter']">
//                 العربية
//               </button>
//               <button className="hover:opacity-70 transition-opacity">
//                 <img src={modeIcon} alt="Dark Mode" className="w-[24px] h-[24px]" />
//               </button>
//             </div>

//             {/* Buttons Group: Gap 24px */}
//             <div className="flex items-center gap-[24px]">
//               {/* Login: Width 90px */}
//               <Button variant="outline" className="w-[90px]">
//                 Log in
//               </Button>
              
//               {/* Get Started: Width 148px */}
//               <Button variant="primary" className="w-[148px] group">
//                 Get Started
//                 <img src={nextIcon} alt="arrow" className="ml-[10px] w-[10px] h-[9px] transition-transform group-hover:translate-x-1" />
//               </Button>
//             </div>
//           </div>

//           {/* Mobile Toggle */}
//           <div className="flex items-center md:hidden">
//             <button onClick={toggleOpen} className="p-2 text-muted hover:text-primary">
//               {isOpen ? <X /> : <Menu />}
//             </button>
//           </div>
//         </div>
//       </div>

//       {/* Mobile Menu */}
//       {isOpen && (
//         <div className="absolute top-[82px] left-0 w-full bg-white border-t shadow-lg md:hidden p-4 flex flex-col gap-4 animate-fade-in">
//            {NAV_LINKS.map((link) => (
//              <a key={link.label} href="#" className="text-muted font-medium py-2 border-b border-gray-50">{link.label}</a>
//            ))}
//            <div className="flex justify-between pt-2 items-center">
//              <span>العربية</span>
//              <img src={modeIcon} alt="Dark Mode" className="w-6 h-6" />
//            </div>
//            <div className="flex flex-col gap-3 mt-2">
//              <Button variant="outline" className="w-full justify-center">Log in</Button>
//              <Button variant="primary" className="w-full justify-center">Get Started</Button>
//            </div>
//         </div>
//       )}
//     </nav>
//   );
// };

// export default Navbar;
import React, { useState, useEffect } from 'react';
import { Menu, X } from 'lucide-react';
import { Link } from 'react-router-dom';
import { useToggle } from '../../hooks/useToggle';
import { NAV_LINKS, BRAND } from '../../data/content';
import Button from '../ui/Button';

import logoImg from '../../assets/logo.svg';
import modeIcon from '../../assets/mode.svg';
import nextIcon from '../../assets/icons_link-next.svg';

const Navbar = () => {
  const [isOpen, toggleOpen] = useToggle(false);
  const [activeLink, setActiveLink] = useState(NAV_LINKS[0].label);
  const [isScrolled, setIsScrolled] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      if (window.scrollY > 10) setIsScrolled(true);
      else setIsScrolled(false);

      const sections = NAV_LINKS.map(link => {
        const id = link.href.replace('#', '');
        return document.getElementById(id);
      });

      const scrollPosition = window.scrollY + 100;

      sections.forEach(section => {
        if (section) {
          const sectionTop = section.offsetTop;
          const sectionHeight = section.offsetHeight;

          if (scrollPosition >= sectionTop && scrollPosition < sectionTop + sectionHeight) {
            const activeLinkData = NAV_LINKS.find(link => link.href === `#${section.id}`);
            if (activeLinkData) {
              setActiveLink(activeLinkData.label);
            }
          }
        }
      });
    };

    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []);

  const handleNavClick = (e, href) => {
    e.preventDefault(); 
    
    const targetId = href.replace('#', '');
    const element = document.getElementById(targetId);

    if (element) {
      const navbarHeight = 82;
      const elementPosition = element.getBoundingClientRect().top + window.scrollY;
      const offsetPosition = elementPosition - navbarHeight;

      window.scrollTo({
        top: offsetPosition,
        behavior: "smooth"
      });
      
      if (isOpen) toggleOpen();
    }
  };

  return (
    <nav className={`bg-white h-[82px] sticky top-0 z-50 flex items-center font-sans transition-shadow duration-300 ${isScrolled ? 'shadow-md' : 'shadow-[0px_4px_10px_rgba(0,0,0,0.05)]'}`}>
      <div className="w-full max-w-[1440px] mx-auto px-6 md:px-[80px]">
        <div className="flex justify-between items-center w-full">
          
          <div className="flex items-center">
             <div 
               className="flex-shrink-0 flex items-center gap-3 cursor-pointer mr-[100px]" 
               onClick={(e) => handleNavClick(e, '#home')}
             >
               <img src={logoImg} alt="PulseX" className="w-[32px] h-[27px] object-contain" />
               <span className="text-main text-[18px] font-bold font-['Poppins'] tracking-tight">{BRAND.name}</span>
             </div>
          </div>

          <div className="hidden md:flex items-center gap-[48px]">
               {NAV_LINKS.map((link) => (
                 <a 
                   key={link.label} 
                   href={link.href} 
                   onClick={(e) => handleNavClick(e, link.href)}
                   className="relative flex flex-col items-center h-[26px] justify-between group cursor-pointer"
                 >
                   <span className={`text-[14px] font-medium transition-colors duration-200 ${activeLink === link.label ? 'text-primary' : 'text-muted group-hover:text-primary'}`}>
                     {link.label}
                   </span>
                   <div className={`h-[2px] rounded-full bg-primary transition-all duration-300 ease-out ${activeLink === link.label ? 'w-full opacity-100' : 'w-0 opacity-0 group-hover:w-1/2 group-hover:opacity-30'}`}></div>
                 </a>
               ))}
             </div>

          <div className="hidden md:flex items-center gap-[56px]">
            <div className="flex items-center gap-6">
              <button className="text-muted hover:text-primary transition-colors text-[14px] font-medium font-['Inter']">العربية</button>
              <button className="hover:opacity-70 transition-opacity"><img src={modeIcon} alt="Dark Mode" className="w-[24px] h-[24px]" /></button>
            </div>

            <div className="flex items-center gap-[24px]">
              <Link to="/login">
                <Button variant="outline" className="w-[90px]">Log in</Button>
              </Link>
            <Link to="/register">
                <Button variant="primary" className="w-[148px] group">
                  Get Started
                  <img src={nextIcon} alt="arrow" className="ml-[10px] w-[10px] h-[9px] transition-transform group-hover:translate-x-1" />
                </Button>
              </Link>
            </div>
          </div>

          <div className="flex items-center md:hidden">
            <button onClick={toggleOpen} className="p-2 text-muted hover:text-primary">
              {isOpen ? <X /> : <Menu />}
            </button>
          </div>
        </div>
      </div>

      {isOpen && (
        <div className="absolute top-[82px] left-0 w-full bg-white border-t shadow-lg md:hidden p-4 flex flex-col gap-4 animate-fade-in">
           {NAV_LINKS.map((link) => (
             <a 
                key={link.label} 
                href={link.href} 
                onClick={(e) => handleNavClick(e, link.href)}
                className={`font-medium py-2 border-b border-gray-50 ${activeLink === link.label ? 'text-primary' : 'text-muted'}`}
             >
               {link.label}
             </a>
           ))}
           
           <div className="flex justify-between pt-2 items-center">
             <span>العربية</span>
             <img src={modeIcon} alt="Dark Mode" className="w-6 h-6" />
           </div>
           <div className="flex flex-col gap-3 mt-2">
             <Link to="/login" className="w-full">
                <Button variant="outline" className="w-full justify-center">Log in</Button>
             </Link>
             <Link to="/register">
                <Button variant="primary" className="w-[148px] group">
                  Get Started
                  <img src={nextIcon} alt="arrow" className="ml-[10px] w-[10px] h-[9px] transition-transform group-hover:translate-x-1" />
                </Button>
              </Link>
              </div>
        </div>
      )}
    </nav>
  );
};

export default Navbar;