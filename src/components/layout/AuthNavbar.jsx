import React from 'react';
import { Link } from 'react-router-dom';
// استدعاء الصورة من المسار بتاعك
import logoImg from '../../assets/logo.svg'; 

const AuthNavbar = () => {
  return (
    <nav className="h-[80px] bg-white w-full flex items-center border-b border-gray-100 sticky top-0 z-50">
      <div className="w-full max-w-[1440px] mx-auto px-6 md:px-[80px] flex items-center justify-between">
        
        {/* Brand Logo with Image */}
        <Link to="/" className="flex items-center gap-3 cursor-pointer group">
          
           <img 
             src={logoImg} 
             alt="PulseX" 
             className="w-[32px] h-[27px] object-contain" 
           />
           <span className="text-[22px] font-bold text-gray-900 font-['Poppins'] tracking-tight group-hover:text-blue-600 transition-colors">
            PulseX
          </span>
        </Link>

        {/* Right Side Link */}
        
      </div>
    </nav>
  );
};

export default AuthNavbar;