import React, { useState } from 'react';
import { motion } from 'framer-motion';
import Container from '../ui/Container';

import doctorImg1 from '../../assets/drAya.svg';
import doctorImg2 from '../../assets/mandr.jpg';
import doctorImg3 from '../../assets/womandr.jpg';

import statusdr from '../../assets/dr/Group 6974.svg';
import star_icon from '../../assets/dr/Star 1.svg';
import elect_icon from '../../assets/dr/Vector-1.svg';
import heart from '../../assets/dr/heart.svg';
import preacher from '../../assets/dr/ri_speed-up-fill.svg';
import save from '../../assets/dr/Vector.svg';
import status from '../../assets/dr/Group 6973.svg';
import pepole from '../../assets/dr/mdi_people.svg';
import chevronRightIcon from '../../assets/dr/arrow-right.svg';
import chevronLeftIcon from '../../assets/dr/arrow-left.svg';

const DOCTORS = [
  {
    id: 1,
    name: "Dr. Aya Fathy Saed",
    role: "Interventional Cardiologist",
    exp: "15+ years",
    rating: "4.9",
    patients: "2,847",
    metrics: { hr: "68 BPM", bp: "120/80", risk: "15%", status: "Normal" }
  },
  {
    id: 2,
    name: "Dr. Omar Khaled",
    role: "Cardiac Surgeon",
    exp: "12+ years",
    rating: "4.8",
    patients: "1,530",
    metrics: { hr: "72 BPM", bp: "118/75", risk: "10%", status: "Excellent" }
  },
  {
    id: 3,
    name: "Dr. Sarah Ahmed",
    role: "Clinical Cardiologist",
    exp: "8+ years",
    rating: "5.0",
    patients: "3,100",
    metrics: { hr: "65 BPM", bp: "122/82", risk: "12%", status: "Normal" }
  }
];

const DOCTOR_IMAGES = {
    1: doctorImg1,
    2: doctorImg2,
    3: doctorImg3,
};

const fadeInUp = {
  hidden: { opacity: 0, y: 60 },
  visible: { opacity: 1, y: 0, transition: { duration: 0.8, ease: [0.22, 1, 0.36, 1] } }
};

const DoctorSection = () => {
  const [currentIndex, setCurrentIndex] = useState(0);

  const nextSlide = () => {
    if (currentIndex < DOCTORS.length - 1) {
      setCurrentIndex(prev => prev + 1);
    }
  };

  const prevSlide = () => {
    if (currentIndex > 0) {
      setCurrentIndex(prev => prev - 1);
    }
  };

  return (
    <section id="doctors" className="pt-24 pb-[200px] bg-[#F9FAFB] overflow-hidden">
      <Container>
        
        <div className="mb-20 text-center mx-auto max-w-[900px]">
          <motion.h2 
            initial="hidden"
            whileInView="visible"
            viewport={{ once: true, margin: "-100px" }}
            variants={fadeInUp}
            className="text-[36px] md:text-[40px] font-bold text-main mb-4 font-display tracking-tight"
          >
            Expert Cardiologists
          </motion.h2>
          
          <motion.p 
            initial="hidden"
            whileInView="visible"
            viewport={{ once: true, margin: "-100px" }}
            variants={fadeInUp}
            className="text-[18px] text-muted font-normal font-sans mb-6"
          >
            Connect with certified heart specialists for personalized care and monitoring
          </motion.p>

          <motion.p 
            initial="hidden"
            whileInView="visible"
            viewport={{ once: true, margin: "-100px" }}
            variants={fadeInUp}
            className="text-[16px] text-muted font-normal font-sans leading-[1.8] mx-auto max-w-[780px]"
          >
            Our AI-powered platform connects you with board-certified cardiologists who specialize in remote monitoring and preventive care. Get expert insights, personalized recommendations, and continuous support for your heart health journey.
          </motion.p>
        </div>

        <motion.div 
          initial="hidden"
          whileInView="visible"
          viewport={{ once: true, margin: "-50px" }}
          variants={fadeInUp}
          className="relative max-w-[1000px] mx-auto" 
        >
          
          <div className="bg-white rounded-[16px] shadow-[0px_0px_3px_0px_rgba(0,0,0,0.08),0px_2px_3px_0px_rgba(0,0,0,0.17)] overflow-hidden relative min-h-[400px]">
            
            <motion.div 
              className="flex"
              animate={{ x: `-${currentIndex * 100}%` }}
              transition={{ type: "spring", stiffness: 300, damping: 30 }}
            >
              {DOCTORS.map((doctor) => (
                <div key={doctor.id} className="w-full flex-shrink-0 px-8 py-10 md:px-16 md:py-10">
                  
                  <div 
                    className="flex flex-col lg:flex-row justify-center items-center gap-12 lg:gap-[208px]"
                  >
                    
                    <div className="flex flex-col gap-6 w-full lg:w-[40%] items-start">
                        <div className="relative w-fit">
                            <img 
                                src={DOCTOR_IMAGES[doctor.id] || doctorImg1} 
                                alt={doctor.name} 
                                className="w-[155px] h-[155px] rounded-full object-cover" 
                            />
                            <img 
                                src={statusdr} 
                                alt="Online Status" 
                                className="absolute bottom-1 right-1 w-5 h-5 border-[2px] border-white rounded-full"
                            />
                        </div>
                        
                        <div className="flex flex-col gap-1 w-full">
                            <div className="flex flex-col gap-2">
                               <h3 className="text-[24px] font-bold text-[#111827] font-['Inter']">{doctor.name}</h3>
                               <p className="text-[16px] font-medium text-[#333CF5] font-['Inter']">{doctor.role}</p>
                            </div>
                            <p className="text-[16px] font-normal text-[#737373] font-['Inter']">{doctor.exp}</p>
                        </div>

                        <div className="flex items-center gap-3">
                            <div className="flex items-center gap-1">
                                <img src={star_icon} alt="Star Rating" className="w-4 h-4" /> 
                                <span className="text-[14px] text-[#111827] font-['Inter']">{doctor.rating}</span>
                            </div>
                            <div className="flex items-center gap-1">
                                <img src={pepole} alt="Patients Count" className="w-4 h-4" />
                                <span className="text-[14px] text-[#737373] font-['Inter']">{doctor.patients}</span>
                            </div>
                        </div>

                        <div className="flex items-center gap-1">
                            <div className="w-1.5 h-1.5 bg-[#22C55E] rounded-full"></div>
                            <span className="text-[14px] font-normal text-[#15A96D font-['Inter']">Available Now</span>
                        </div>
                    </div>

                    <div className="w-full lg:w-[60%] flex flex-col gap-6 items-start">
                      
                      <div className="flex items-center gap-2">
                          <img src={elect_icon} alt="Activity" className="w-5 h-5" />
                          <h4 className="text-[18px] font-semibold text-[#111827] font-['Inter']">Patient Metrics</h4>
                      </div>
                      
                      <div className="flex flex-col gap-3 w-full">
                        
                        <div className="flex justify-between items-center border-b border-gray-50 pb-2">
                            <div className="flex items-center gap-2">
                                <img src={heart} alt="Heart Rate" className="w-4 h-4" />
                                <span className="text-[14px] text-[#737373] font-['Inter'] font-normal">Heart Rate</span>
                            </div>
                            <span className="text-[16px] font-medium text-[#111827] font-['Inter']">{doctor.metrics.hr}</span>
                        </div>

                        <div className="flex justify-between items-center border-b border-gray-50 pb-2">
                            <div className="flex items-center gap-2">
                                <img src={preacher} alt="Blood Pressure" className="w-4 h-4" /> 
                                <span className="text-[14px] text-[#737373] font-['Inter'] font-normal">Blood Pressure</span>
                            </div>
                            <span className="text-[16px] font-medium text-[#111827] font-['Inter']">{doctor.metrics.bp}</span>
                        </div>

                        <div className="flex justify-between items-center border-b border-gray-50 pb-2">
                            <div className="flex items-center gap-2">
                                <img src={save} alt="Risk Score" className="w-4 h-4" />
                                <span className="text-[14px] text-[#737373] font-['Inter'] font-normal">Risk Score</span>
                            </div>
                            <span className="text-[16px] font-medium text-[#111827] font-['Inter']">{doctor.metrics.risk}</span>
                        </div>

                        <div className="flex justify-between items-center pb-2">
                            <div className="flex items-center gap-2">
                                <img src={status} alt="Health Status" className="w-4 h-4" />
                                <span className="text-[14px] text-[#737373] font-['Inter'] font-normal">Status</span>
                            </div>
                            <span className={`text-[16px] font-medium font-['Inter'] ${doctor.metrics.status === "Excellent" ? 'text-[#15A96D]' : doctor.metrics.status === "Normal" ? 'text-[#15A96D]' : 'text-[#EF4444]'}`}>
                              {doctor.metrics.status}
                            </span>
                        </div>
                      </div>
                      
                      <div className="w-full h-[112px] bg-white rounded-lg shadow-[0px_0px_3px_0px_rgba(0,0,0,0.08),0px_1px_1px_0px_rgba(0,0,0,0.17)] border border-[#737373]/20 relative overflow-hidden p-4">
                          <div className="flex justify-between items-center mb-4">
                            <span className="text-[12px] text-[#737373] font-normal font-['Inter']">ECG Recording</span>
                            <span className="text-[12px] text-[#22C55E] font-medium font-['Inter']">Live</span>
                          </div>
                          
                          <div className="absolute top-[40px] left-0 w-full px-4"> 
                              <svg className="w-full h-[50px] text-[##059669]" viewBox="0 0 300 50" fill="none" preserveAspectRatio="none">
                                  <line x1="0" y1="35" x2="300" y2="35" stroke="#737373" strokeWidth="1" opacity="0.3" />
                                  <path 
                                      d="M0 35 L40 35 L45 30 L50 10 L55 45 L60 30 L65 35 L300 35" 
                                      stroke="#059669"
                                      strokeWidth="2" 
                                      strokeLinecap="round" 
                                      strokeLinejoin="round" 
                                      className="animate-ecg" 
                                      strokeDasharray="1000" 
                                      strokeDashoffset="1000"
                                  />
                                  <line x1="50" y1="0" x2="50" y2="50" stroke="#22C55E" strokeWidth="1" opacity="0.3" />
                              </svg>
                          </div>
                      </div>

                    </div>

                  </div>
                </div>
              ))}
            </motion.div>
          </div>

          {currentIndex < DOCTORS.length - 1 && (
            <button 
              onClick={nextSlide}
              className="absolute top-1/2 -translate-y-1/2 -right-6 md:-right-16 w-12 h-12 bg-white rounded-full shadow-[0px_0px_1px_0px_rgba(0,0,0,0.25)] border-[0.2px] border-[#111827] flex items-center justify-center hover:scale-105 transition-all z-20"
            >
              <img src={chevronRightIcon} alt="Next" className="w-5 h-5 text-[#111827]" />
            </button>
          )}

          {currentIndex > 0 && (
            <button 
              onClick={prevSlide}
              className="absolute top-1/2 -translate-y-1/2 -left-6 md:-left-16 w-12 h-12 bg-white rounded-full shadow-[0px_0px_1px_0px_rgba(0,0,0,0.25)] border-[0.2px] border-[#111827] flex items-center justify-center hover:scale-105 transition-all z-20"
            >
              <img src={chevronLeftIcon} alt="Previous" className="w-5 h-5 text-[#111827]" />
            </button>
          )}

          <div className="flex justify-center gap-2 mt-8">
            {DOCTORS.map((_, index) => (
              <div 
                key={index}
                className={`w-2 h-2 rounded-full transition-colors duration-300 cursor-pointer 
                  ${currentIndex === index ? 'bg-[#333CF5]' : 'bg-[#D4D4D8]'}`}
                onClick={() => setCurrentIndex(index)}
              ></div>
            ))}
          </div>

        </motion.div>

      </Container>
    </section>
  );
};

export default DoctorSection;