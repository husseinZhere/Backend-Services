import React, { useState } from 'react';
import Container from '../ui/Container';
import SectionHeader from '../ui/SectionHeader';

import Image1 from '../../assets/fetureimage1.png'; 
import Image2 from '../../assets/feature2.png'; 
import Image3 from '../../assets/feature3.png'; 
import Image4 from '../../assets/feature4.png'; 
import Image5 from '../../assets/feture5.png'; 

import icontop from '../../assets/icon_top_fetuer.svg'; 
import iconbutton from '../../assets/icon_button_fetuer.svg'; 

import Heart from '../../assets/Feature icons/Vector-4.svg'; 
import QrCode from '../../assets/Feature icons/Group-1.svg'; 
import LinkIcon from '../../assets/Feature icons/Vector-7.svg'; 
import FileText from '../../assets/Feature icons/iconamoon_file-document.svg'; 
import Video from '../../assets/Feature icons/solar_videocamera-record-linear.svg'; 

import Heart_active from '../../assets/Feature icons/Vector-9.svg'; 
import QrCode_active from '../../assets/Feature icons/Group-2.svg'; 
import LinkIcon_active from '../../assets/Feature icons/Vector.svg'; 
import FileText_active from '../../assets/Feature icons/iconamoon_file-document-1.svg'; 
import Video_active from '../../assets/Feature icons/solar_videocamera-record-linear-1.svg';

import Heart_muted from '../../assets/Feature icons/heartmuted.svg'; 
import QrCode_muted from '../../assets/Feature icons/Group.svg'; 
import LinkIcon_muted from '../../assets/Feature icons/Vector-1.svg'; 
import FileText_muted from '../../assets/Feature icons/iconamoon_file-document-2.svg'; 
import Video_muted from '../../assets/Feature icons/solar_videocamera-record-linear-2.svg'; 

const FEATURE_COLORS = [
    { main: 'bg-indigo-600', text: 'text-indigo-600', hex: '#6366F1' }, 
    { main: 'bg-red-500', text: 'text-red-500', hex: '#E94242' },
    { main: 'bg-amber-600', text: 'text-amber-600', hex: '#D0791D' }, 
    { main: 'bg-cyan-600', text: 'text-cyan-600', hex: '#0891B2' }, 
    { main: 'bg-emerald-500', text: 'text-emerald-500', hex: '#12D385' }, 
];

const FEATURES = [
  {
    id: 0,
    title: "AI Heart Risk Score",
    description: "Advanced machine learning algorithms analyze your vital signs, lifestyle factors, and medical history to provide a comprehensive risk assessment with 95% accuracy.",
    stats: [
      { value: "95%", label: "Accuracy" },
      { value: "50+", label: "Data points" },
      { value: "Real-time", label: "Updates" }
    ],
    icon: { base: Heart, active: Heart_active, muted: Heart_muted },
    image: Image1 
  },
  {
    id: 1,
    title: "Emergency QR Codes",
    description: "Instant access to your complete medical profile for emergency responders. Critical information available in seconds when every moment counts.",
    stats: [
      { value: "< 5 sec", label: "Response Time" },
      { value: "Complete", label: "Medical Data" },
      { value: "24/7", label: "Availability" }
    ],
    icon: { base: QrCode, active: QrCode_active, muted: QrCode_muted },
    image: Image2
  },
  {
    id: 2,
    title: "Medication Reminders", 
    description: "Smart medication management with personalized reminders. drug interaction alerts, and adherence tracking to optimize your treatment Plan.",
    stats: [
      { value: "98%", label: "Adherence Rate" },
      { value: "Custom", label: "Reminders" },
      { value: "Monitored", label: "Interactions" }
    ],
    icon: { base: LinkIcon, active: LinkIcon_active, muted: LinkIcon_muted },
    image: Image3
  },
  {
    id: 3,
    title: "Full Medical Records",
    description: "Comprehensive digital health records with secure cloud storage, easy sharing with healthcare providers, and complete medical history tracking.",
    stats: [
      { value: "95%", label: "Accuracy" },
      { value: "50+", label: "Data points" },
      { value: "Real-time", label: "Updates" }
    ],
    icon: { base: FileText, active: FileText_active, muted: FileText_muted },
    image: Image4
  },
  {
    id: 4,
    title: "Doctor Follow-ups",
    description: "Virtual consultations with certified cardiologists, regular check-ins, and personalized care plans tailored to your specific heart health needs.",
    stats: [
      { value: "24/7", label: "Availability" },
      { value: "50+", label: "Specialists" },
      { value: "15+", label: "Languages" }
    ],
    icon: { base: Video, active: Video_active, muted: Video_muted },
    image: Image5
  }
];

const Features = () => {
  const [activeTab, setActiveTab] = useState(0); 
  const currentFeature = FEATURES[activeTab];
  const currentFeatureColor = FEATURE_COLORS[activeTab];

  return (
    <section id="features" className="py-24 bg-gray-50">
      <Container>
      
        <div data-aos="fade-up"> 
          <SectionHeader 
            title="Comprehensive Heart Health Features" 
            subtitle="Advanced AI-powered tools for complete cardiovascular monitoring and care" 
          />
        </div>

        <div className="bg-white rounded-[32px] overflow-hidden relative min-h-[680px] shadow-xl transition-all duration-300">
          
          <div className="grid lg:grid-cols-2 h-full">
            
            <div 
              className="p-10 md:p-16 pb-48 flex flex-col justify-center relative z-10"
              key={activeTab}
            > 
              
              <div className="flex items-center gap-3 mb-6 animate-fade-in-up"> 
                <div className="w-14 h-14 rounded-full bg-white border border-gray-200 shadow-sm flex items-center justify-center">
                   <img src={currentFeature.icon.base} alt="icon" className="w-6 h-6" />
                </div>
                <h3 className="text-3xl font-bold text-gray-900 font-['Poppins']">
                  {currentFeature.title}
                </h3>
              </div>

              <p className="text-lg text-gray-600 font-normal font-['Poppins'] leading-7 mb-10 min-h-[80px] animate-fade-in-up delay-100">
                {currentFeature.description}
              </p>

              <div className="flex items-center gap-14 mb-16 animate-fade-in-up delay-200">
                {currentFeature.stats.map((stat, index) => (
                  <div key={index} className="text-center w-28">
                    <p className="text-xl font-bold text-gray-900 font-['Inter']">{stat.value}</p>
                    <p className="text-base text-gray-600 font-normal font-['Inter']">{stat.label}</p>
                  </div>
                ))}
              </div>

              <div className="w-full max-w-md animate-fade-in-up delay-300">
                <div className="flex justify-between items-center mb-2 text-[14px] text-gray-600 font-medium">
                  <span className="text-sm font-normal font-['Inter']">Feature {activeTab + 1} of 5</span>
                  <span className="text-base font-normal font-['Inter']">{(activeTab + 1) * 20}%</span>
                </div>
                <div className="h-1.5 w-full bg-gray-200 rounded-full overflow-hidden"> 
                  <div 
                    className={`h-full bg-blue-600 rounded-full transition-all duration-500 ease-out`}
                    style={{ width: `${(activeTab + 1) * 20}%` }}
                  ></div>
                </div>
              </div>

            </div>

            <div className="relative h-full min-h-[400px]">
              <img 
                key={activeTab} 
                src={currentFeature.image} 
                alt={currentFeature.title}
                className="w-[552.72px] h-96 rounded-lg shadow-[0px_0px_3px_0px_rgba(0,0,0,0.08)] shadow-[0px_2px_3px_0px_rgba(0,0,0,0.17)] object-cover absolute top-[60px] left-1/5"
              />
              
              <div className="absolute top-10 right-8 w-10 h-10 p-1.5 bg-white rounded-3xl border border-gray-300 flex items-center justify-center animate-pulse-slow">
                  <img src={icontop} alt="Top Decoration" className="w-6 h-6" />
              </div>

              <div className="absolute bottom-10 left-[-20px] w-10 h-10 p-1.5 bg-white rounded-3xl border border-gray-300 flex items-center justify-center animate-pulse-slow delay-500">
                  <img src={iconbutton} alt="Bottom Decoration" className="w-6 h-6" />
              </div>
            </div>

          </div>

          <div className="absolute bottom-8 left-1/2 transform -translate-x-1/2 flex items-center gap-4 z-20">
            
            {FEATURES.map((feature, index) => {
              const isActive = activeTab === index;
              const color = FEATURE_COLORS[index];
              
              const iconSrc = isActive ? feature.icon.active : feature.icon.muted;

              let iconBgClass = isActive 
                  ? `${color.main} shadow-xl ring-4 ring-white scale-110`
                  : 'bg-white border border-gray-100 shadow-md hover:scale-105';
              
              return (
                <button
                  key={index}
                  onClick={() => setActiveTab(index)}
                  className={`
                    w-14 h-14 rounded-full flex items-center justify-center transition-all duration-300 
                    ${iconBgClass}
                  `}
                >
                  <img src={iconSrc} alt="nav icon" className="w-6 h-6" />
                </button>
              );
            })}

          </div>

        </div>

      </Container>
    </section>
  );
};

export default Features;