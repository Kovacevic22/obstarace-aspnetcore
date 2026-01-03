import LogoImg from "../../assets/Logo.png"
import {Link} from "react-router";

export function UserNavbar(){
    return(
        <div className="fixed top-0 left-0 right-0 z-50 bg-[var(--color-dark)]/90 backdrop-blur-md border-b border-white/5">
            <div className="max-w-7xl mx-auto px-6 h-20 flex justify-between items-center">

                <Link to={"/"}>
                    <div className="flex items-center group cursor-pointer">
                        <div className="relative w-12 h-12 md:w-14 md:h-14 flex items-center justify-center">
                            <div className="absolute inset-0 bg-[var(--color-accent)] opacity-0 group-hover:opacity-20 blur-lg transition-all duration-500 rounded-full"></div>
                            <img src={LogoImg} alt="Logo" className="relative z-10 w-full h-full object-contain" />
                        </div>
                        <div className="ml-3 border-l border-white/10 pl-3">
                            <div className="text-white font-black text-lg uppercase tracking-tighter leading-none">Obsta<span className="text-[var(--color-accent)]">Race</span></div>
                            <div className="text-[8px] text-[var(--color-secondary)] font-bold uppercase tracking-[0.3em] mt-1">Athlete</div>
                        </div>
                    </div>
                </Link>

                <ul className="hidden lg:flex items-center gap-8 text-[11px] font-bold uppercase tracking-[0.2em] text-white/80">
                    <li><Link to={"/"} className="hover:text-[var(--color-accent)] transition-all">Home</Link></li>
                    <li><Link to={"/races"} className="hover:text-[var(--color-accent)] transition-all">Races</Link></li>
                    <li><Link to={""} className="hover:text-[var(--color-accent)] transition-all">Obstacles</Link></li>
                    <li><Link to={""} className="hover:text-[var(--color-accent)] transition-all">My Registrations</Link></li>
                </ul>
                <div className="flex items-center gap-6">
                    <div className="text-[10px] font-black uppercase tracking-widest text-[var(--color-accent)] hover:text-white cursor-pointer transition-all">
                        Profile
                    </div>
                    <div className="text-[10px] font-black uppercase tracking-widest text-white/30 hover:text-red-500 cursor-pointer transition-all">
                        Logout
                    </div>
                </div>
            </div>
        </div>
    )
}
export default UserNavbar;