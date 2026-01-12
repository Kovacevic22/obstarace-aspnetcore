import LogoImg from "../../assets/Logo.png"
import {Link} from "react-router";
function PublicNavbar() {
    return (
        <div className="fixed top-0 left-0 right-0 z-50 bg-dark/90 backdrop-blur-md border-b border-white/5">
            <div className="max-w-7xl mx-auto px-6 h-16 md:h-20 flex justify-between items-center transition-all duration-300">
                <Link to={"/"}>
                    <div className="flex items-center cursor-pointer group">
                        <div className="flex items-center cursor-pointer group">
                            <div className="relative w-12 h-12 md:w-14 md:h-14 flex items-center justify-center">
                                <div className="absolute inset-0 bg-accent opacity-0 group-hover:opacity-20 blur-lg transition-opacity duration-500 rounded-full"></div>
                                <img src={LogoImg} alt="Logo" className="relative z-10 w-full h-full object-contain"/>
                            </div>
                            <div className="ml-3 flex flex-col justify-center border-l border-white/10 pl-3">
                                <div className="text-white text-lg md:text-xl font-black uppercase leading-none tracking-tighter">
                                    Obsta<span className="text-accent">Race</span>
                                </div>
                                <div className="text-[7px] md:text-[8px] text-secondary font-bold uppercase tracking-[0.3em] mt-0.5">
                                    Balkan Series
                                </div>
                            </div>
                        </div>
                    </div>
                </Link>
                <div className="hidden md:flex items-center gap-10">
                    <ul className="flex gap-8 text-[10px] font-bold uppercase tracking-[0.2em] text-light/80">
                        <li className="relative group">
                            <Link to={"/"} className="hover:text-white transition-all">Home</Link>
                            <div className="absolute -bottom-1 left-0 w-0 h-px bg-accent group-hover:w-full transition-all"></div>
                        </li>
                        <li className="relative group">
                            <Link to={"/races"} className="hover:text-white transition-all">Races</Link>
                            <div className="absolute -bottom-1 left-0 w-0 h-px bg-accent group-hover:w-full transition-all"></div>
                        </li>
                        <li className="relative group">
                            <Link to={""} className="hover:text-white transition-all">About</Link>
                            <div className="absolute -bottom-1 left-0 w-0 h-px bg-accent group-hover:w-full transition-all"></div>
                        </li>
                    </ul>
                    <div className="flex items-center gap-2 pl-4 border-l border-white/10">
                        <Link to={"/login"}>
                            <div className="px-4 py-2 text-[10px] font-black uppercase tracking-widest text-white hover:text-accent cursor-pointer transition-all">
                                Login
                            </div>
                        </Link>
                        <Link to={"/register"}>
                            <div className="px-6 py-2 bg-accent text-dark text-[10px] font-black uppercase tracking-widest cursor-pointer hover:bg-white transition-all active:scale-95">
                                Register
                            </div>
                        </Link>
                    </div>
                </div>
                <div className="md:hidden flex flex-col gap-1 items-end cursor-pointer p-2 group">
                    <div className="w-6 h-0.5 bg-white transition-all group-hover:w-8"></div>
                    <div className="w-8 h-0.5 bg-accent"></div>
                    <div className="w-4 h-0.5 bg-white transition-all group-hover:w-8"></div>
                </div>
            </div>
        </div>
    )
}
export default PublicNavbar;