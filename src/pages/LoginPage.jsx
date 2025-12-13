import React, { useState } from 'react';
import { Check } from 'lucide-react'; 
import { Link, useNavigate } from 'react-router-dom'; 
import loginImg from '../assets/Login Photo.png'; 
import loginIcon from '../assets/Grouplogin.svg'; 

const LoginPage = () => {
  const [formData, setFormData] = useState({ email: '', password: '' });
  const [errors, setErrors] = useState({}); 
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });

    if (errors[name]) {
      setErrors({ ...errors, [name]: '' });
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault(); 
    let newErrors = {};
    let isValid = true;

    if (!formData.email) {
      newErrors.email = "Email address is required";
      isValid = false;
    } else if (!/\S+@\S+\.\S+/.test(formData.email)) {
      newErrors.email = "Please enter a valid email address";
      isValid = false;
    }

    if (!formData.password) {
      newErrors.password = "Password is required";
      isValid = false;
    } else if (formData.password.length < 6) {
      newErrors.password = "Password must be at least 6 characters";
      isValid = false;
    }

    setErrors(newErrors);

    if (isValid) {
      console.log("Form Submitted", formData);
    }
  };

  return (
    <div className="w-full h-screen bg-gray-50 overflow-hidden flex relative">
 
        <img 
            src={loginImg} 
            alt="Login Visual" 
            className="absolute left-0 top-0 w-[58%] h-full object-cover hidden lg:block" 
        />

        <div className="w-full lg:w-[42%] h-full absolute right-0 top-0 bg-gray-50 flex items-center justify-center">
            <div className="w-full max-w-[480px] px-8 py-6 bg-white rounded-[24px] shadow-[0px_4px_12px_rgba(0,0,0,0.08)] border border-gray-100 mx-4">
                
                <div className="text-center mb-8">
                    <h2 className="text-3xl font-bold text-gray-900 mb-2 font-['Inter']">Welcome Back</h2>
                    <p className="text-gray-500 text-base font-normal font-['Inter']">Sign in to access your heartcare account</p>
                </div>

                <form onSubmit={handleSubmit} className="flex flex-col gap-6">
                    
                    <div className="flex flex-col gap-2">
                        <label className={`text-sm font-normal flex gap-1 ${errors.email ? 'text-red-600' : 'text-gray-800'}`}>
                            Email Address <span className="text-red-600">*</span>
                        </label>
                        
                        <input 
                            type="email" 
                            name="email"
                            value={formData.email}
                            onChange={handleChange}
                            placeholder="Enter your email" 
                            className={`w-full h-[50px] px-4 rounded-[24px] text-sm outline-none transition-all
                                ${errors.email 
                                    ? 'bg-white border border-red-500 text-red-600 placeholder:text-red-300' 
                                    : 'bg-gray-50 text-gray-700 border-none placeholder:text-gray-400 focus:bg-white focus:shadow-md' 
                                }`}
                        />
                        {errors.email && <span className="text-xs text-red-600 mt-1">{errors.email}</span>}
                    </div>

                    <div className="flex flex-col gap-2">
                        <label className={`text-sm font-normal flex gap-1 ${errors.password ? 'text-red-600' : 'text-gray-800'}`}>
                            Password <span className="text-red-600">*</span>
                        </label>
                        
                        <div className="relative">
                             <input 
                                type="password" 
                                name="password"
                                value={formData.password}
                                onChange={handleChange}
                                placeholder="Enter your Password" 
                                className={`w-full h-[50px] px-4 rounded-[24px] text-sm outline-none transition-all
                                    ${errors.password 
                                        ? 'bg-white border border-red-500 text-red-600 placeholder:text-red-300' 
                                        : 'bg-gray-50 text-gray-700 border-none placeholder:text-gray-400 focus:bg-white focus:shadow-md'
                                    }`}
                            />
                        </div>
                        {errors.password && <span className="text-xs text-red-600 mt-1">{errors.password}</span>}
                    </div>

                    <div className="flex justify-between items-center mt-1">
                        <label className="flex items-center gap-2 cursor-pointer group select-none">
                            <input type="checkbox" className="peer sr-only" />
                            <div className="w-5 h-5 border-2 border-gray-300 rounded flex items-center justify-center transition-all duration-200
                                hover:border-gray-400
                                peer-checked:bg-[#333CF5] peer-checked:border-[#333CF5]
                                peer-checked:[&>svg]:opacity-100"> 
                               <Check className="w-3.5 h-3.5 text-white opacity-0 transition-opacity duration-200" strokeWidth={3} />
                            </div>
                            <span className="text-[14px] font-medium text-gray-800">Remember me</span>
                        </label>
                        
                        <Link to="/forgot-password" className="text-[14px] text-[#333CF5] hover:underline">
                            Forgot Password?
                        </Link>
                    </div>

                    <button type="submit" className="w-full h-[50px] bg-[#333CF5] text-white rounded-[24px] font-medium text-[14px] hover:bg-[#282eb5] transition-colors flex items-center justify-center gap-2 shadow-md group">
                        Continue to Dashboard
                        <img 
                            src={loginIcon} 
                            alt="Arrow" 
                            className="w-3 h-3 transition-transform group-hover:translate-x-1" 
                        />
                    </button>
                </form>

                <div className="w-full h-[1px] bg-gray-200 my-8"></div>

                <div className="flex justify-center gap-1 text-[14px]">
                      <span className="text-gray-500">Don't have an account?</span>
                      <Link to="/signup" className="text-[#333CF5] font-medium hover:underline">
                        Create Account
                      </Link>
                </div>

            </div>
        </div>
    </div>
  );
};

export default LoginPage;