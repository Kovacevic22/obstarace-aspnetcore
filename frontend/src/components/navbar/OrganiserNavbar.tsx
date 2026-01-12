import LogoImg from "../../assets/Logo.png"
import {Link} from "react-router";
import authService from "../../services/authService.ts";

export function OrganiserNavbar(){
    return(
        <div className="fixed top-0 left-0 right-0 z-50 bg-dark/95 backdrop-blur-md border-b border-secondary/30">
            <div className="max-w-7xl mx-auto px-6 h-18 flex justify-between items-center">
                <Link to={"/"}>
                    <div className="flex items-center group cursor-pointer">
                        <div className="relative w-12 h-12 flex items-center justify-center">
                            <div className="absolute inset-0 bg-secondary opacity-0 group-hover:opacity-20 blur-lg transition-all duration-500 rounded-full"></div>
                            <img src={LogoImg} alt="Logo" className="relative z-10 w-full h-full object-contain" />
                        </div>
                        <div className="ml-3 text-white font-black text-base uppercase tracking-tighter">
                            Organiser<span className="text-secondary">Hub</span>
                        </div>
                    </div>
                </Link>

                <div className="flex items-center gap-4">
                    <div className="hidden sm:block text-[9px] font-black uppercase tracking-[0.3em] text-white/20 italic">
                        Access: Root Organiser
                    </div>
                    <div onClick={authService.logout} className="px-5 py-2 bg-secondary text-white text-[10px] font-black uppercase tracking-widest cursor-pointer hover:bg-[var(--color-mud)] transition-all active:scale-95">
                        Logout
                    </div>
                </div>
            </div>
        </div>
    )
}
export default OrganiserNavbar;