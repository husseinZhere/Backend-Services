import React from 'react';
import { useNavigate } from 'react-router-dom';

const PasswordChanged = () => {
  const navigate = useNavigate();

  return (
    <div className="w-full min-h-screen bg-gray-50 flex items-center justify-center font-['Inter'] overflow-hidden">
      
      <div className="w-96 flex flex-col justify-start items-center gap-6">
        
        <div className="w-20 h-20 bg-[#00C55E] rounded-full flex items-center justify-center shadow-[0px_0px_3px_0px_rgba(0,0,0,0.08)] shadow-[0px_2px_3px_0px_rgba(0,0,0,0.17)]">
           <svg width="40" height="40" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
            <path d="M5 13L9 17L19 7" stroke="white" strokeWidth="3" strokeLinecap="round" strokeLinejoin="round"/>
           </svg>
        </div>

        <div className="self-stretch flex flex-col justify-start items-center gap-3">
           
           <div className="self-stretch text-center text-[#010218] text-xl font-medium font-['Inter'] leading-8 tracking-wide">
             Password changed
           </div>

           <div className="self-stretch text-center text-[#757575] text-base font-normal font-['Roboto'] leading-6 tracking-tight">
             Your password has been changed successfully
           </div>
        </div>

        <button
          onClick={() => navigate('/login')}
          className="self-stretch h-12 px-4 py-3 bg-[#333CF5] hover:bg-[#282eb5] rounded-3xl inline-flex justify-center items-center gap-2.5 transition-colors shadow-sm cursor-pointer border-none"
        >
          <div className="text-center text-white text-sm font-medium font-['Inter']">Log in</div>
        </button>

      </div>
    </div>
  );
};

export default PasswordChanged;