import {Link} from "react-router"
import LogoImg from "../../assets/Logo.png"
import authService from "../../services/authService.ts";

export function AdminNavbar(){
    return(
        <div className="fixed top-0 left-0 right-0 z-50 bg-dark/95 backdrop-blur-md border-b border-white/5">
            <div className="max-w-7xl mx-auto px-6 h-18 flex justify-between items-center">
                <Link to="/">
                    <div className="flex items-center group cursor-pointer">
                        <div className="relative w-10 h-10 flex items-center justify-center">
                            <div className="absolute inset-0 bg-accent opacity-0 group-hover:opacity-20 blur-lg transition-all duration-500 rounded-full"></div>
                            <img src={LogoImg} alt="Logo" className="relative z-10 w-full h-full object-contain" />
                        </div>
                        <div className="ml-3 text-white font-black text-base uppercase tracking-tighter">
                            Admin<span className="text-accent">Panel</span>
                        </div>
                    </div>
                </Link>
                <div className="flex items-center gap-6">
                    <div className="hidden sm:block text-[9px] font-black uppercase tracking-[0.3em] text-white/20 italic">
                        Access: Root Admin
                    </div>
                    <button
                        onClick={() => authService.logout()}
                        className="px-4 py-1.5 border border-white/10 text-white/40 text-[10px] font-black uppercase tracking-widest cursor-pointer hover:text-secondary hover:border-secondary/50 transition-all active:scale-95"
                    >
                        [ Logout ]
                    </button>
                </div>
            </div>
        </div>
    )
}
export default AdminNavbar;