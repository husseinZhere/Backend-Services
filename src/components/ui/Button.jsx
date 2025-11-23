import React from 'react';

const styles = {
  base: "h-[48px] px-6 rounded-full text-[14px] font-medium flex items-center justify-center transition-all duration-300 whitespace-nowrap",
  
  variants: {
    primary: "bg-primary text-white hover:bg-primary-hover shadow-md hover:shadow-lg border border-transparent",
  
    outline: "bg-white text-muted border border-gray-200 hover:bg-primary hover:text-white hover:border-primary shadow-sm hover:shadow-md",
    
    ghost: "text-primary hover:bg-blue-50"
  }
};

const Button = ({ children, variant = 'primary', icon: Icon, className = '', ...props }) => {
  return (
    <button 
      className={`${styles.base} ${styles.variants[variant]} ${className}`} 
      {...props}
    >
      {children}
      {Icon && <Icon className="ml-2 w-4 h-4" />}
    </button>
  );
};

export default Button;