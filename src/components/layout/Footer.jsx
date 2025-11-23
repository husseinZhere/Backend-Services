import React from 'react';
import Container from '../ui/Container';
import { ArrowRight, Rocket, Twitter, Facebook, Instagram, Linkedin, Mail } from 'lucide-react';

import logoImg from '../../assets/logo.svg';
import email from '../../assets/futter_icons/email.svg';
import X from '../../assets/futter_icons/X.svg';
import facebook from '../../assets/futter_icons/facebook.svg';
import linkedin from '../../assets/futter_icons/linkedin.svg';
import instagram from '../../assets/futter_icons/instagram.svg';

const Footer = () => {
  return (
    <footer className="bg-white pt-24 pb-10 border-t border-gray-100 relative overflow-hidden">
      <Container>
        
        <div className="flex flex-col items-center mb-24 relative z-10">
           <button className="px-10 py-5 rounded-full flex items-center gap-4 bg-gradient-to-r from-[#0913C3] to-[#FF0000] shadow-lg hover:shadow-2xl hover:scale-105 transition-all duration-300 group">
             <Rocket className="w-6 h-6 text-white" />
             <span className="font-bold text-[18px] text-white tracking-wide">
               Start Your Journey Now
             </span>
             <ArrowRight className="w-5 h-5 text-white group-hover:translate-x-1 transition-transform" />
           </button>

           <div className="mt-16 grid grid-cols-2 md:grid-cols-4 gap-8 md:gap-24 text-center">
             <div>
               <p className="text-[36px] font-bold text-[#333CF5] mb-1">95%</p>
               <p className="text-[16px] text-muted font-medium">Success Rate</p>
             </div>
             <div>
               <p className="text-[36px] font-bold text-[#333CF5] mb-1">10K+</p>
               <p className="text-[16px] text-muted font-medium">Patients Helped</p>
             </div>
             <div>
               <p className="text-[36px] font-bold text-[#333CF5] mb-1">24/7</p>
               <p className="text-[16px] text-muted font-medium">Support Available</p>
             </div>
             <div>
               <p className="text-[36px] font-bold text-[#333CF5] mb-1">50+</p>
               <p className="text-[16px] text-muted font-medium">Expert Doctors</p>
             </div>
           </div>
        </div>

        <div className="h-[1px] w-full bg-gray-100 mb-16"></div>

        <div className="grid grid-cols-1 md:grid-cols-12 gap-12 md:gap-8 mb-16">
          
          <div className="md:col-span-5 flex flex-col gap-6">
             <div className="flex items-center gap-3">
               <img src={logoImg} alt="PulseX" className="w-8 h-8 object-contain" />
               <span className="text-[24px] font-bold text-main font-display tracking-tight">PulseX</span>
             </div>
             <p className="text-[16px] text-muted leading-[1.6] max-w-[360px] font-sans">
               PulseX is revolutionizing cardiovascular care with advanced AI-powered monitoring, risk assessment, and personalized treatment recommendations for better heart health outcomes.
             </p>
             <p className="text-[16px] text-muted font-medium italic">
               Empowering heart health through AI innovation
             </p>
          </div>
          
          <div className="hidden md:block md:col-span-1"></div>

          <div className="md:col-span-2">
            <h4 className="text-[18px] font-bold text-main mb-6 font-display">Quick Links</h4>
            <ul className="space-y-4">
              {['Home', 'About', 'Contact Us'].map((item) => (
                <li key={item}>
                  <a href="#" className="text-[16px] text-muted hover:text-[#333CF5] transition-colors font-medium">
                    {item}
                  </a>
                </li>
              ))}
            </ul>
            <div className="mt-8">
               <button className="bg-[#333CF5] text-white px-6 py-3 rounded-full text-[14px] font-medium flex items-center gap-2 hover:bg-[#282eb5] transition-colors shadow-md group">
                 Get Started
                 <ArrowRight className="w-4 h-4 transition-transform group-hover:translate-x-1" />
               </button>
            </div>
          </div>
          
          <div className="md:col-span-2">
            <h4 className="text-[18px] font-bold text-main mb-6 font-display">Support</h4>
            <ul className="space-y-4">
              {['Documentation', 'Community'].map((item) => (
                <li key={item}>
                  <a href="#" className="text-[16px] text-muted hover:text-[#333CF5] transition-colors font-medium">
                    {item}
                  </a>
                </li>
              ))}
            </ul>
          </div>
        </div>

        <div className="flex flex-col md:flex-row justify-between items-end border-t border-gray-100 pt-8 gap-8">
          <div className="flex items-center gap-6">
            <a href="#" className="text-main hover:text-[#333CF5] transition-colors"> <img src={instagram} alt="PulseX" className="w-6 h-6 object-contain" /></a>
            <a href="#" className="text-main hover:text-[#333CF5] transition-colors"> <img src={linkedin} alt="PulseX" className="w-6 h-6 object-contain" /></a>
            <a href="#" className="text-main hover:text-[#333CF5] transition-colors"> <img src={facebook} alt="PulseX" className="w-6 h-6 object-contain" /></a>
            <a href="#" className="text-main hover:text-[#333CF5] transition-colors"> <img src={X} alt="PulseX" className="w-6 h-6 object-contain" /></a>
          </div>

          <div className="flex flex-col gap-2">
            <span className="text-[16px] text-main font-medium">Email</span>
            <a href="mailto:support@pulseX.health" className="text-[16px] text-muted hover:text-[#333CF5] transition-colors flex items-center gap-2">
              <div className="w-4 h-4"> <img src={email} alt="PulseX" className="w-4 h-4 object-contain" /></div>
              support@pulseX.health
            </a>
          </div>
        </div>

        <div className="mt-12 flex flex-col md:flex-row justify-between items-center text-[14px] text-muted pt-8 border-t border-gray-100">
          <p>© 2025 PulseX. All rights reserved.</p>
          <div className="flex items-center gap-8 mt-4 md:mt-0">
            <div className="flex items-center gap-2">
              <div className="w-2 h-2 bg-emerald-500 rounded-full animate-pulse"></div>
              <span>All systems operational</span>
            </div>
            <div className="flex items-center gap-2">
              <div className="w-1.5 h-1.5 bg-gray-400 rounded-full"></div>
              <span>Version 2.1.0</span>
            </div>
          </div>
        </div>

      </Container>
    </footer>
  );
};

export default Footer;